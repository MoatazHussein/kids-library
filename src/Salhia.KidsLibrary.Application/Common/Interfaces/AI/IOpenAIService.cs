namespace Salhia.KidsLibrary.Application.Common.Interfaces.AI;

public interface IOpenAIService
{
    /// <summary>
    /// Generates a story with slides using OpenAI
    /// </summary>
    /// <param name="storyName">Name of the story</param>
    /// <param name="heroName">Name of the hero character</param>
    /// <param name="slidesCount">Number of slides to generate (5-8)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Story generation result with slides</returns>
    Task<StoryGenerationResult> GenerateStoryWithSlidesAsync(
        string storyName,
        string heroName,
        int slidesCount,
        CancellationToken cancellationToken = default);
}
