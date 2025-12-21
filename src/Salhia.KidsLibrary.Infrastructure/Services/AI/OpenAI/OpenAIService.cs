using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Salhia.KidsLibrary.Application.Common.Interfaces.AI;
using Salhia.KidsLibrary.Infrastructure.Services.AI.Configuration;
using Salhia.KidsLibrary.Infrastructure.Services.AI.OpenAI.Models;

namespace Salhia.KidsLibrary.Infrastructure.Services.AI.OpenAI;

public class OpenAIService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAIOptions _options;
    private readonly ILogger<OpenAIService> _logger;

    public OpenAIService(
        HttpClient httpClient,
        IOptions<AIServiceOptions> options,
        ILogger<OpenAIService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value.OpenAI;
        _logger = logger;

        // Configure HttpClient
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    public async Task<StoryGenerationResult> GenerateStoryWithSlidesAsync(
        string storyName,
        string heroName,
        int slidesCount,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        
        _logger.LogInformation(
            "Generating story with OpenAI: StoryName={StoryName}, HeroName={HeroName}, SlidesCount={SlidesCount}",
            storyName, heroName, slidesCount);

        var prompt = BuildStoryPrompt(storyName, heroName, slidesCount);
        var request = new OpenAIRequest
        {
            Model = _options.Model,
            Temperature = _options.Temperature,
            Messages = new List<ChatMessage>
            {
                new() { Role = "system", Content = "You are a professional children's story writer. Generate concise, engaging stories." },
                new() { Role = "user", Content = prompt }
            },
            ResponseFormat = new ResponseFormat { Type = "json_object" },
            MaxTokens = _options.MaxTokens,
        };

        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage? response = null;
        for (int attempt = 1; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                response = await _httpClient.PostAsync("/v1/chat/completions", content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    break;
                }

                _logger.LogWarning(
                    "OpenAI request failed. Attempt {Attempt}/{MaxRetries}. StatusCode: {StatusCode}",
                    attempt, _options.MaxRetries, response.StatusCode);

                if (attempt < _options.MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
                }
            }
            catch (Exception ex) when (attempt < _options.MaxRetries)
            {
                _logger.LogWarning(ex,
                    "OpenAI request exception. Attempt {Attempt}/{MaxRetries}",
                    attempt, _options.MaxRetries);

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
            }
        }

        if (response == null || !response.IsSuccessStatusCode)
        {
            var errorContent = response != null ? await response.Content.ReadAsStringAsync(cancellationToken) : "No response";
            _logger.LogError("OpenAI API failed after {MaxRetries} attempts. Error: {Error}",
                _options.MaxRetries, errorContent);
            throw new HttpRequestException($"OpenAI API request failed: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogDebug("OpenAI raw response: {Response}", responseContent);

        var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
        if (openAIResponse?.Choices == null || openAIResponse.Choices.Count == 0)
        {
            _logger.LogError("OpenAI response has no choices");
            throw new InvalidOperationException("OpenAI returned empty response");
        }

        // Log token usage
        if (openAIResponse.Usage != null)
        {
            _logger.LogInformation(
                "Token usage: Prompt={PromptTokens}, Completion={CompletionTokens}, Total={TotalTokens}",
                openAIResponse.Usage.PromptTokens,
                openAIResponse.Usage.CompletionTokens,
                openAIResponse.Usage.TotalTokens);
        }

        var messageContent = openAIResponse.Choices[0].Message.Content;
        var storyResponse = JsonSerializer.Deserialize<OpenAIStoryResponse>(messageContent);

        if (storyResponse?.Slides == null || storyResponse.Slides.Count == 0)
        {
            _logger.LogError("OpenAI response has no slides. Content: {Content}", messageContent);
            throw new InvalidOperationException("OpenAI did not return any slides");
        }

        stopwatch.Stop();
        _logger.LogInformation(
            "Successfully generated {SlideCount} slides from OpenAI in {ElapsedMs}ms)",
            storyResponse.Slides.Count, 
            stopwatch.ElapsedMilliseconds);

        // Map to Application layer model
        var result = new StoryGenerationResult
        {
            Slides = storyResponse.Slides.Select(s => new StorySlideContent
            {
                Title = s.Title,
                Description = s.Description,
                ImagePrompt = s.ImagePrompt
            }).ToList()
        };

        return result;
    }

    private static string BuildStoryPrompt(string storyName, string heroName, int slidesCount)
    {
        return $@"You are a professional children's story writer. Create a story for children in Arabic.

Story Information:
- Story Title: {storyName}
- Hero Name: {heroName}
- Number of Slides: {slidesCount}

Task: Create {slidesCount} slides for the story. Each slide must contain:
1. A short title (optional) - in Arabic
2. Story description (2-3 sentences) - in Arabic
3. Image description (image prompt) - in English - a detailed description of a scene featuring the main character

Important Rules:
- The story must be appropriate for children (ages 4-10)
- Each slide must feature the main character in the scene
- The image prompt must be in ENGLISH ONLY and describe the scene with clear details
- The image prompt should describe the main character as 'the child', 'the main character', or 'the hero' (do NOT use the actual hero name in the image prompt)
- CRITICAL: Each image prompt MUST END with this exact sentence: ""Use the face from the provided reference image for the main character, preserving all facial features.""
- Focus on describing the character's pose, clothing, actions, environment, and scene details
- Use child-friendly and colorful illustration style descriptions
- The character's face should be clearly visible and facing forward or at an angle
- Use an engaging narrative style suitable for children

Return the result in JSON format only like this:
{{
  ""slides"": [
    {{
      ""title"": ""عنوان الشريحة"",
      ""description"": ""وصف القصة بالعربية (2-3 جمل)"",
      ""imagePrompt"": ""A colorful children's illustration showing the main character [action/pose] in [setting]. The character is wearing [clothing] and [doing activity]. [Describe environment and mood]. Digital art style, vibrant colors, child-friendly. Use the face from the provided reference image for the main character, preserving all facial features.""
    }}
  ]
}}";
    }
}
