using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

using Salhia.KidsLibrary.Infrastructure.Services.Pdf.Models;

namespace Salhia.KidsLibrary.Infrastructure.Services.Pdf;

public class PdfService(
    IRepository<CustomStory> customStoryRepository,
    IRepository<AIStory> aiStoryRepository,
    ILogger<PdfService> logger,
    IHttpClientFactory httpClientFactory
    ) : IPdfService
{
    public async Task<byte[]> GenerateCustomStoryPdfAsync(string customStoryId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Generating PDF for custom story {StoryId}", customStoryId);

        // Fetch the custom story with items
        var customStory = await customStoryRepository.GetByIdAsync(
            customStoryId,
            cancellationToken,
            [cs => cs.CustomStoryItems, cs => cs.CreatedByUser]);

        if (customStory == null)
        {
            throw new NotFoundException(nameof(CustomStory), customStoryId);
        }

        // Sort items by CreatedAt
        var storyItems = customStory.CustomStoryItems
            .OrderBy(item => item.CreatedAt)
            .Select(item => new PdfStoryItem
            {
                Title = item.Title,
                Description = item.Description,
                ImageUrl = item.ImageUrl
            })
            .ToList();

        var pdfData = new PdfStoryData
        {
            Title = customStory.Title,
            AuthorName = customStory.AuthorName,
            Description = customStory.Description,
            Items = storyItems
        };

        return GenerateCustomStoryLayout(pdfData);
    }

    public async Task<byte[]> GenerateAIStoryPdfAsync(string aiStoryId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Generating PDF for AI story {StoryId}", aiStoryId);

        var aiStory = await aiStoryRepository.GetByIdAsync(
            aiStoryId,
            cancellationToken,
            [s => s.AIStorySlides, s => s.CreatedByUser]);

        if (aiStory == null)
        {
            throw new NotFoundException(nameof(AIStory), aiStoryId);
        }

        var storyItems = aiStory.AIStorySlides
            .OrderBy(item => item.Index)
            .Select(item => new PdfStoryItem
            {
                Title = item.Title,
                Description = item.Description,
                ImageUrl = item.ImageUrl
            })
            .ToList();

        var pdfData = new PdfStoryData
        {
            Title = aiStory.StoryName,
            AuthorName = aiStory.CreatedByUser?.FirstName ?? "AI Author",
            HeroName = aiStory.HeroName,
            Items = storyItems,
            IsAIStory = true
        };

        return GenerateAIStoryLayout(pdfData);
    }

    private byte[] GenerateCustomStoryLayout(PdfStoryData data)
    {
        logger.LogInformation("Generating Custom Story PDF with {PageCount} pages for story '{Title}'",
            data.Items.Count, data.Title);

        return GeneratePdfDocument(data);
    }

    private byte[] GenerateAIStoryLayout(PdfStoryData data)
    {
        logger.LogInformation("Generating AI Story PDF with {PageCount} pages for story '{Title}'",
            data.Items.Count, data.Title);

        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Simplified Arabic"));
                
                page.ContentFromRightToLeft();

                // No header for AI stories

                page.Content()
                    .Column(column =>
                    {
                        // COVER PAGE - Use first slide (Index 0)
                        if (data.Items.Any())
                        {
                            var coverSlide = data.Items[0]; // Index 0 = cover
                            
                            // Story name in bold
                            column.Item()
                                .PaddingTop(40)
                                .AlignCenter()
                                .Text(data.Title)
                                .FontSize(24)
                                .Bold();
                            
                            // Hero name in bold
                            if (!string.IsNullOrEmpty(data.HeroName))
                            {
                                column.Item()
                                    .PaddingTop(10)
                                    .AlignCenter()
                                    .Text($"البطل: {data.HeroName}")
                                    .FontSize(20)
                                    .Bold();
                            }
                            
                            // Cover image
                            if (!string.IsNullOrEmpty(coverSlide.ImageUrl))
                            {
                                RenderImage(column, coverSlide.ImageUrl);
                            }
                            
                            // Cover description
                            if (!string.IsNullOrEmpty(coverSlide.Description))
                            {
                                column.Item()
                                    .PaddingTop(20)
                                    .AlignRight()
                                    .Text(coverSlide.Description)
                                    .FontSize(14);
                            }

                            // Story slides (Index 1+)
                            foreach (var item in data.Items.Skip(1))
                            {
                                column.Item().PageBreak();
                                
                                // Add top spacing for better layout
                                column.Item().PaddingTop(20);
                                
                                // Slide title with decorative border (optional)
                                if (!string.IsNullOrEmpty(item.Title))
                                {
                                    column.Item()
                                        .PaddingBottom(15)
                                        .BorderBottom(2)
                                        .BorderColor("#4A90E2")
                                        .AlignRight()
                                        .Text(item.Title)
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor("#2C3E50");
                                }

                                // Slide image with spacing
                                if (!string.IsNullOrEmpty(item.ImageUrl))
                                {
                                    column.Item().PaddingTop(20).PaddingBottom(20);
                                    RenderImage(column, item.ImageUrl);
                                }

                                // Slide description with better spacing and formatting
                                if (!string.IsNullOrEmpty(item.Description))
                                {
                                    column.Item()
                                        .PaddingTop(20)
                                        .PaddingLeft(10)
                                        .PaddingRight(10)
                                        .AlignRight()
                                        .Text(item.Description)
                                        .FontSize(14)
                                        .LineHeight(1.6f)
                                        .FontColor("#34495E");
                                }
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("صفحة ");
                        x.CurrentPageNumber();
                        x.Span(" من ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();

        return pdfBytes;
    }

    private byte[] GeneratePdfDocument(PdfStoryData data)
    {
        // Generate PDF using QuestPDF
        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial").DirectionFromRightToLeft());

                page.Header()
                    .Text(data.Title)
                    .FontSize(20)
                    .Bold()
                    .AlignCenter();

                page.Content()
                    .Column(column =>
                    {
                        // Cover page
                        column.Item().PaddingTop(20).AlignCenter().Text($"المؤلف: {data.AuthorName}").FontSize(16);
                        
                        if (!string.IsNullOrEmpty(data.Description))
                        {
                            column.Item().PaddingTop(20).AlignRight().Text(data.Description).FontSize(12);
                        }

                        // Story items (one per page)
                        foreach (var item in data.Items)
                        {
                            column.Item().PageBreak();
                            
                            // Item title
                            if (!string.IsNullOrEmpty(item.Title))
                            {
                                column.Item().AlignRight().Text(item.Title).FontSize(18).Bold();
                            }

                            // Item image (if exists)
                            if (!string.IsNullOrEmpty(item.ImageUrl))
                            {
                                try
                                {
                                    // Check if it's an external URL or local path
                                    if (IsExternalUrl(item.ImageUrl))
                                    {
                                        // Download image from external URL
                                        var imageBytes = DownloadImageAsync(item.ImageUrl).GetAwaiter().GetResult();
                                        
                                        if (imageBytes != null && imageBytes.Length > 0)
                                        {
                                            column.Item()
                                                .PaddingTop(10)
                                                .PaddingBottom(10)
                                                .AlignCenter()
                                                .MaxHeight(300)
                                                .Image(imageBytes);
                                        }
                                    }
                                    else
                                    {
                                        // Load from local storage
                                        var imagePath = GetLocalImagePath(item.ImageUrl);
                                        
                                        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                        {
                                            column.Item()
                                                .PaddingTop(10)
                                                .PaddingBottom(10)
                                                .AlignCenter()
                                                .MaxHeight(300)
                                                .Image(imagePath);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex, "Failed to load image {ImageUrl}", item.ImageUrl);
                                }
                            }

                            // Item description
                            if (!string.IsNullOrEmpty(item.Description))
                            {
                                column.Item()
                                    .PaddingTop(10)
                                    .AlignRight()
                                    .Text(item.Description)
                                    .FontSize(12);
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("صفحة ");
                        x.CurrentPageNumber();
                        x.Span(" من ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();

        return pdfBytes;
    }

    private void RenderImage(ColumnDescriptor column, string imageUrl)
    {
        try
        {
            if (IsExternalUrl(imageUrl))
            {
                var imageBytes = DownloadImageAsync(imageUrl).GetAwaiter().GetResult();
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    column.Item()
                        .PaddingTop(10)
                        .PaddingBottom(10)
                        .AlignCenter()
                        .MaxHeight(300)
                        .Image(imageBytes);
                }
            }
            else
            {
                var imagePath = GetLocalImagePath(imageUrl);
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    column.Item()
                        .PaddingTop(10)
                        .PaddingBottom(10)
                        .AlignCenter()
                        .MaxHeight(300)
                        .Image(imagePath);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load image {ImageUrl}", imageUrl);
        }
    }

    private bool IsExternalUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<byte[]?> DownloadImageAsync(string imageUrl)
    {
        try
        {
            logger.LogInformation("Downloading external image from {ImageUrl}", imageUrl);
            
            using var httpClient = httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            var response = await httpClient.GetAsync(imageUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to download image {ImageUrl}. StatusCode: {StatusCode}", 
                    imageUrl, response.StatusCode);
                return null;
            }

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            
            logger.LogInformation("Successfully downloaded image {ImageUrl} ({Size} bytes)", 
                imageUrl, imageBytes.Length);
            
            return imageBytes;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading image from URL: {ImageUrl}", imageUrl);
            return null;
        }
    }

    private string GetLocalImagePath(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return string.Empty;

        try
        {
            // Get the current working directory (where the API is running)
            var currentDirectory = Directory.GetCurrentDirectory();
            
            logger.LogInformation("Current working directory: {Directory}", currentDirectory);
            
            // Parse the URL to extract the path portion
            Uri? uri;
            string relativePath;
            
            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out uri))
            {
                // It's a full URL like "https://domain.com/Storage/Images/file.jpg"
                // Extract path: /Storage/Images/file.jpg
                relativePath = uri.AbsolutePath;
            }
            else
            {
                // It's already a relative path like "/Storage/Images/file.jpg"
                relativePath = imageUrl;
            }
            
            // Remove leading slash and normalize path separators
            relativePath = relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            
            // Combine with current directory to get full local path
            var fullPath = Path.Combine(currentDirectory, relativePath);
            
            logger.LogInformation("Image URL resolution: {ImageUrl} -> {RelativePath} -> {FullPath}", 
                imageUrl, relativePath, fullPath);
            
            return fullPath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error resolving image path for URL: {ImageUrl}", imageUrl);
            return string.Empty;
        }
    }

}
