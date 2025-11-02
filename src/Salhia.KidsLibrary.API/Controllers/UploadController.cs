using Salhia.KidsLibrary.Application.Features.Images.Commands.UploadImage;
using Salhia.KidsLibrary.Application.Features.Files.Commands.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController(IMediator mediator) : ControllerBase
{
    [HttpPost("image")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageCommand command)
    {
        var url = await mediator.Send(command);
        return Ok(new { url });
    }

    [HttpPost("file")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand command)
    {
        var url = await mediator.Send(command);
        return Ok(new { url });
    }
}
