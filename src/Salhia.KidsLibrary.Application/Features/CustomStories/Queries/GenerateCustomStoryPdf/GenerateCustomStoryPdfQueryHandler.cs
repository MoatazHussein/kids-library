using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GenerateCustomStoryPdf;

public class GenerateCustomStoryPdfQueryHandler(IPdfService pdfService) : IRequestHandler<GenerateCustomStoryPdfQuery, byte[]>
{
    public async Task<byte[]> Handle(GenerateCustomStoryPdfQuery request, CancellationToken cancellationToken)
    {
        return await pdfService.GenerateCustomStoryPdfAsync(request.Id, cancellationToken);
    }
}
