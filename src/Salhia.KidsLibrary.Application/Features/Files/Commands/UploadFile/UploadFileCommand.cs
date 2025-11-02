using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Salhia.KidsLibrary.Application.Features.Files.Commands.UploadFile;

public class UploadFileCommand : IRequest<string> 
{
    [FromForm]
    public IFormFile File { get; set; } = default!;
}
