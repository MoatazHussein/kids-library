using MediatR;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.StoryShares.Commands.ShareStory;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class StorySharesController(IMediator mediator) : ControllerBase
{
    [HttpPost("Share")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Share([FromBody] ShareStoryCommand command)
    {
        await mediator.Send(command);
        return Ok(new { message = "Story shared successfully" });
    }
}
