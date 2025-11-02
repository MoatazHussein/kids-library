using Salhia.KidsLibrary.Application.Common.Interfaces;
using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Files.Commands.UploadFile;

public class UploadFileCommandHandler(IStorageService storageService) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        return await storageService.SaveFileAsync(request.File, cancellationToken);
    }
}
