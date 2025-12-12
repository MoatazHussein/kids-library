using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GenerateAIStoryPdf;

public class GenerateAIStoryPdfQueryHandler(IPdfService pdfService) : IRequestHandler<GenerateAIStoryPdfQuery, byte[]>
{
    public async Task<byte[]> Handle(GenerateAIStoryPdfQuery request, CancellationToken cancellationToken)
    {
        return await pdfService.GenerateAIStoryPdfAsync(request.Id, cancellationToken);
    }
}
