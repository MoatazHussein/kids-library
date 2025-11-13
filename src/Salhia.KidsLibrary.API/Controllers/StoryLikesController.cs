using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.StoryLikes.Commands.ToggleStoryLike;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class StoryLikesController(IMediator mediator) : ControllerBase
{
    [HttpPost("Toggle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Toggle([FromBody] ToggleStoryLikeCommand command)
    {
        await mediator.Send(command);
        return Ok(new { message = "Story like toggled successfully" });
    }
}
