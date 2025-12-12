namespace Salhia.KidsLibrary.Application.Common.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateCustomStoryPdfAsync(string customStoryId, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateAIStoryPdfAsync(string aiStoryId, CancellationToken cancellationToken = default);
}
