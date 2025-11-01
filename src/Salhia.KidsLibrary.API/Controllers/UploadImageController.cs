using Salhia.KidsLibrary.Application.Features.Images.Commands.UploadImage;
using Salhia.KidsLibrary.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadImageController(IMediator mediator) : ControllerBase
{
    [HttpPost("image")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageCommand command)
    {
        var url = await mediator.Send(command);
        return Ok(new { url });
    }
}
