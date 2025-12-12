using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.AI;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Commands.GenerateAIStory;

public class GenerateAIStoryCommandHandler(
    IRepository<AIStory> aiStoryRepository,
    IRepository<AIStorySlide> aiStorySlideRepository,
    IRepository<CustomStory> customStoryRepository,
    IOpenAIService openAIService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IRepository<SystemSetting> systemSettingRepository,
    ILogger<GenerateAIStoryCommandHandler> logger) : IRequestHandler<GenerateAIStoryCommand, string>
{
    public async Task<string> Handle(GenerateAIStoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate CustomStory exists
        var customStory = await customStoryRepository.GetByIdAsync(request.CustomStoryId, cancellationToken);
        if (customStory == null)
        {
            logger.LogWarning("CustomStory with Id {CustomStoryId} not found", request.CustomStoryId);
            throw new AppException($"CustomStory with Id {request.CustomStoryId} not found");
        }

        // 2. Check Rate Limit
        var currentUserId = currentUserService.UserId ?? string.Empty;
        
        // Get the first (and only) settings row
        var settings = await systemSettingRepository.FirstOrDefaultAsync(x => true, cancellationToken);

        if (settings is null)
        {
            throw new AppException("System settings not configured for AI story limits");
        }

        var limitDate = DateTime.UtcNow.AddDays(-settings.AIStoryLimitDays);
        var recentStoriesCount = await aiStoryRepository.CountAsync(
            s => s.CreatedBy == currentUserId && s.CreatedAt >= limitDate, 
            cancellationToken);

        if (recentStoriesCount >= settings.AIStoryLimitCount)
        {
            logger.LogWarning("User {UserId} exceeded AI story limit. Count: {Count}, Limit: {Limit}", 
                currentUserId, recentStoriesCount, settings.AIStoryLimitCount);
            throw new AppException($"You have reached the limit of {settings.AIStoryLimitCount} AI stories every {settings.AIStoryLimitDays} days.");
        }

        // 3. Generate random slides count (5-8)
        var random = new Random();
        var slidesCount = random.Next(5, 9); // 5 to 8 inclusive

        logger.LogInformation(
            "Starting AI story generation. CustomStoryId={CustomStoryId}, StoryName={StoryName}, HeroName={HeroName}, SlidesCount={SlidesCount}",
            request.CustomStoryId, request.StoryName, request.HeroName, slidesCount);

        // 4. Create AIStory record
        var aiStory = new AIStory
        {
            StoryName = request.StoryName,
            HeroName = request.HeroName,
            HeroImageUrl = request.HeroImageUrl,
            SlidesCount = slidesCount,
            CustomStoryId = request.CustomStoryId,
            CreatedBy = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

        await aiStoryRepository.AddAsync(aiStory, cancellationToken);

        // 4. Call OpenAI to generate story content
        var storyResponse = await openAIService.GenerateStoryWithSlidesAsync(
            request.StoryName,
            request.HeroName,
            slidesCount,
            cancellationToken);

        // 5. Create AIStorySlide records with status "Pending"
        var slides = new List<AIStorySlide>();
        for (int i = 0; i < storyResponse.Slides.Count; i++)
        {
            var slideDto = storyResponse.Slides[i];
            var slide = new AIStorySlide
            {
                Index = i + 1,
                Title = slideDto.Title,
                Description = slideDto.Description,
                ImagePrompt = slideDto.ImagePrompt,
                ImageUrl = string.Empty, // Will be filled by background service
                Status = AIStorySlideStatus.Pending,
                AIStoryId = aiStory.Id,
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            slides.Add(slide);
            await aiStorySlideRepository.AddAsync(slide, cancellationToken);
        }

        // 6. Save all changes
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Successfully created AIStory {AIStoryId} with {SlideCount} slides in Pending status",
            aiStory.Id, slides.Count);

        return aiStory.Id;
    }
}
