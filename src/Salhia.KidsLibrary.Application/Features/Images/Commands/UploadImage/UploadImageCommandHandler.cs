using Salhia.KidsLibrary.Application.Common.Interfaces;
using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Images.Commands.UploadImage;

public class UploadImageCommandHandler(IStorageService storageService) : IRequestHandler<UploadImageCommand, string>
{
    public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        return await storageService.SaveImageAsync(request.File, cancellationToken);
    }
}
