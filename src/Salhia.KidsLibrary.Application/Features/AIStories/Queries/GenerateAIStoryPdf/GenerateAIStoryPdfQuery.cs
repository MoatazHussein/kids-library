using MediatR;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GenerateAIStoryPdf;

public record GenerateAIStoryPdfQuery(string Id) : IRequest<byte[]>;
