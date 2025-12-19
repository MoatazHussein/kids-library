namespace Salhia.KidsLibrary.Application.Common.Interfaces.AI;

public interface IFalAIService
{
    Task<string> GenerateImageAsync(
        string heroImageUrl,
        string prompt,
        CancellationToken cancellationToken = default);
}
