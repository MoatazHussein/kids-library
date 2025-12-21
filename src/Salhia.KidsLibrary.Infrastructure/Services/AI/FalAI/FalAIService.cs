using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Salhia.KidsLibrary.Application.Common.Interfaces.AI;
using Salhia.KidsLibrary.Infrastructure.Services.AI.Configuration;
using Salhia.KidsLibrary.Infrastructure.Services.AI.FalAI.Models;

namespace Salhia.KidsLibrary.Infrastructure.Services.AI.FalAI;

public class FalAIService : IFalAIService
{
    private readonly HttpClient _httpClient;
    private readonly FalAIOptions _options;
    private readonly ILogger<FalAIService> _logger;

    public FalAIService(
        HttpClient httpClient,
        IOptions<AIServiceOptions> options,
        ILogger<FalAIService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value.FalAI;
        _logger = logger;

        // Configure HttpClient
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Key", _options.ApiKey);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    public async Task<string> GenerateImageAsync(
        string heroImageUrl,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Generating image with Fal AI. Hero image: {HeroImageUrl}, Original prompt length: {Length} characters",
            heroImageUrl, prompt.Length);

        var request = new FalAIImageRequest
        {
            ImageUrl = heroImageUrl,
            Prompt = prompt,
            Width = _options.DefaultWidth,
            Height = _options.DefaultHeight,
            IdentityStrength = _options.IdentityStrength
        };

        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage? response = null;
        for (int attempt = 1; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                response = await _httpClient.PostAsync("/fal-ai/instant-character", content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    break;
                }

                _logger.LogWarning(
                    "Fal AI request failed. Attempt {Attempt}/{MaxRetries}. StatusCode: {StatusCode}",
                    attempt, _options.MaxRetries, response.StatusCode);

                if (attempt < _options.MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
                }
            }
            catch (Exception ex) when (attempt < _options.MaxRetries)
            {
                _logger.LogWarning(ex,
                    "Fal AI request exception. Attempt {Attempt}/{MaxRetries}",
                    attempt, _options.MaxRetries);

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
            }
        }

        if (response == null || !response.IsSuccessStatusCode)
        {
            var errorContent = response != null ? await response.Content.ReadAsStringAsync(cancellationToken) : "No response";
            _logger.LogError("Fal AI API failed after {MaxRetries} attempts. Error: {Error}",
                _options.MaxRetries, errorContent);
            throw new HttpRequestException($"Fal AI API request failed: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogDebug("Fal AI raw response: {Response}", responseContent);

        var falAIResponse = JsonSerializer.Deserialize<FalAIImageResponse>(responseContent);
        if (falAIResponse?.Images == null || falAIResponse.Images.Count == 0)
        {
            _logger.LogError("Fal AI response has no images");
            throw new InvalidOperationException("Fal AI returned empty response");
        }

        var generatedImage = falAIResponse.Images[0];
        var imageUrl = generatedImage.Url;

        stopwatch.Stop();

        // Log timing information
        if (falAIResponse.Timings != null)
        {
            _logger.LogInformation(
                "Image generated successfully. Total: {ElapsedMs}ms, Inference: {InferenceMs}ms, URL: {Url}",
                stopwatch.ElapsedMilliseconds,
                (int)(falAIResponse.Timings.Inference * 1000),
                imageUrl);
        }
        else
        {
            _logger.LogInformation(
                "Image generated successfully in {ElapsedMs}ms. URL: {Url}",
                stopwatch.ElapsedMilliseconds,
                imageUrl);
        }

        return imageUrl;
    }

}
