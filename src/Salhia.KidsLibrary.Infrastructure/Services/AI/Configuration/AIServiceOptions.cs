namespace Salhia.KidsLibrary.Infrastructure.Services.AI.Configuration;

public class AIServiceOptions
{
    public const string SectionName = "AIServices";

    public OpenAIOptions OpenAI { get; set; } = new();
    public FalAIOptions FalAI { get; set; } = new();
}

public class OpenAIOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.openai.com/v1";
    public string Model { get; set; } = "gpt-4o-mini";
    public double Temperature { get; set; } = 0.5;
    public int MaxRetries { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 60;
    public int MaxTokens { get; set; } = 2000;
}

public class FalAIOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://fal.run/fal-ai/face-to-sticker";
    public int DefaultWidth { get; set; } = 1280;
    public int DefaultHeight { get; set; } = 720;
    public double IdentityStrength { get; set; } = 0.85;
    public int MaxRetries { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 60;
}
