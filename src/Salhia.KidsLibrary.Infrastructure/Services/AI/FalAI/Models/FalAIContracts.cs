using System.Text.Json.Serialization;

namespace Salhia.KidsLibrary.Infrastructure.Services.AI.FalAI.Models;

/// <summary>
/// All Fal AI API request/response contracts
/// </summary>

#region Request Models

public class FalAIImageRequest
{
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = default!;
    
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = default!;
    
    [JsonPropertyName("width")]
    public int Width { get; set; } = 1280;
    
    [JsonPropertyName("height")]
    public int Height { get; set; } = 720;
    
    [JsonPropertyName("identity_strength")]
    public double IdentityStrength { get; set; } = 0.85;
}

#endregion

#region Response Models

public class FalAIImageResponse
{
    [JsonPropertyName("images")]
    public List<GeneratedImage> Images { get; set; } = [];
    
    [JsonPropertyName("timings")]
    public ResponseTimings? Timings { get; set; }
    
    [JsonPropertyName("seed")]
    public long? Seed { get; set; }
    
    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }
}

public class GeneratedImage
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("content_type")]
    public string ContentType { get; set; } = default!;
}

public class ResponseTimings
{
    [JsonPropertyName("inference")]
    public double Inference { get; set; }
}

#endregion
