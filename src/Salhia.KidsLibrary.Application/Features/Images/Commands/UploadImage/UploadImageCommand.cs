using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Salhia.KidsLibrary.Application.Features.Images.Commands.UploadImage;

public class UploadImageCommand : IRequest<string> 
{
    [FromForm]
    public IFormFile File { get; set; } = default!;
}
