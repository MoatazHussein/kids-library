using System.Text.Json.Serialization;

namespace Salhia.KidsLibrary.Infrastructure.Services.AI.OpenAI.Models;

/// <summary>
/// All OpenAI API request/response contracts
/// </summary>

#region Request Models

public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = "gpt-4o-mini";
    
    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = [];
    
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0.5;
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }=2000;
    
    [JsonPropertyName("response_format")]
    public ResponseFormat? ResponseFormat { get; set; }
}

public class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = default!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = default!;
}

public class ResponseFormat
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "json_object";
}

#endregion

#region Response Models

public class OpenAIResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;
    
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = [];
    
    [JsonPropertyName("usage")]
    public TokenUsage? Usage { get; set; }
}

public class Choice
{
    [JsonPropertyName("message")]
    public ChatMessage Message { get; set; } = default!;
    
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = default!;
}

public class TokenUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }
    
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

#endregion

#region Story Generation Models

public class OpenAIStoryResponse
{
    [JsonPropertyName("slides")]
    public List<StorySlideDto> Slides { get; set; } = [];
}

public class StorySlideDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;
    
    [JsonPropertyName("imagePrompt")]
    public string ImagePrompt { get; set; } = default!;
}

#endregion
