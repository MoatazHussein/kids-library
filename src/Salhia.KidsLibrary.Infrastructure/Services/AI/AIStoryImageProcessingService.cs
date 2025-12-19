using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces.AI;
using Salhia.KidsLibrary.Application.Services.AIStoryImageProcessing;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Domain.Exceptions;
using Salhia.KidsLibrary.Infrastructure.Persistence;
using System.Diagnostics;

namespace Salhia.KidsLibrary.Infrastructure.Services.AI;

public class AIStoryImageProcessingService : IAIStoryImageProcessingService
{
    private readonly AppDbContext _context;
    private readonly IFalAIService _falAIService;
    private readonly ILogger<AIStoryImageProcessingService> _logger;
    private static readonly SemaphoreSlim _semaphore = new(3, 3); // 3 concurrent requests

    public AIStoryImageProcessingService(
        AppDbContext context,
        IFalAIService falAIService,
        ILogger<AIStoryImageProcessingService> logger)
    {
        _context = context;
        _falAIService = falAIService;
        _logger = logger;
    }

    public async Task ProcessStoryImmediatelyAsync(string storyId, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        
        _logger.LogDebug("Starting immediate processing for story {StoryId}", storyId);
        
        // Get ONLY slides for this specific story
        var pendingSlides = await _context.AIStorySlides
            .Include(s => s.AIStory)
            .Where(s => s.AIStoryId == storyId && s.Status == AIStorySlideStatus.Pending)
            .OrderBy(s => s.Index)
            .ToListAsync(cancellationToken);

        if (!pendingSlides.Any())
        {
            _logger.LogWarning("No pending slides found for story {StoryId}", storyId);
            return;
        }

        _logger.LogInformation("Processing {Count} slides for story {StoryId}", 
            pendingSlides.Count, storyId);

        // Mark all as Generating
        foreach (var slide in pendingSlides)
        {
            slide.Status = AIStorySlideStatus.Generating;
        }
        await _context.SaveChangesAsync(cancellationToken);

        // Process slides in parallel with controlled concurrency
        var tasks = pendingSlides.Select(slide => ProcessSlideAsync(slide, cancellationToken)).ToList();
        await Task.WhenAll(tasks);

        stopwatch.Stop();
        _logger.LogInformation("Completed immediate processing for story {StoryId} in {ElapsedMs}ms", 
            storyId, stopwatch.ElapsedMilliseconds);
    }

    // NEW: Retry a specific failed slide
    public async Task RetrySlideAsync(string slideId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrying slide {SlideId}", slideId);
        
        var slide = await _context.AIStorySlides
            .Include(s => s.AIStory)
            .FirstOrDefaultAsync(s => s.Id == slideId, cancellationToken);

        if (slide == null)
        {
            _logger.LogWarning("Slide {SlideId} not found", slideId);
            throw new AppException($"Slide with ID {slideId} not found");
        }

        if (slide.Status == AIStorySlideStatus.Ready)
        {
            _logger.LogWarning("Slide {SlideId} is already ready", slideId);
            throw new AppException("This slide has already been generated successfully");
        }

        // Reset status to Generating
        slide.Status = AIStorySlideStatus.Generating;
        slide.ImageUrl = string.Empty; // Clear any previous failed attempt
        await _context.SaveChangesAsync(cancellationToken);

        // Process the slide
        await ProcessSlideAsync(slide, cancellationToken);
        
        _logger.LogInformation("Retry completed for slide {SlideId}", slideId);
    }

    private async Task ProcessSlideAsync(AIStorySlide slide, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        
        try
        {
            var slideStopwatch = Stopwatch.StartNew();
            
            _logger.LogDebug("Processing slide {SlideId} (Index {Index}) for AIStory {AIStoryId}", 
                slide.Id, slide.Index, slide.AIStoryId);

            // Generate image using Fal AI
            var imageUrl = await _falAIService.GenerateImageAsync(
                slide.AIStory.HeroImageUrl,
                slide.ImagePrompt,
                cancellationToken);

            // Validate image URL
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                slide.ImageUrl = imageUrl;
                slide.Status = AIStorySlideStatus.Ready;
                
                slideStopwatch.Stop();
                _logger.LogInformation("Successfully generated image for slide {SlideId} in {ElapsedMs}ms", 
                    slide.Id, slideStopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogWarning("Generated empty image URL for slide {SlideId}", slide.Id);
                slide.Status = AIStorySlideStatus.Failed;
                slide.ImageUrl = string.Empty;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate image for slide {SlideId}", slide.Id);
            
            slide.Status = AIStorySlideStatus.Failed;
            slide.ImageUrl = string.Empty;
        }
        finally
        {
            _semaphore.Release();
        }

        // Save changes for this slide
        await _context.SaveChangesAsync(cancellationToken);
    }
}
