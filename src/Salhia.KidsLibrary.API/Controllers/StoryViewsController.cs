using MediatR;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.API.Helpers;
using Salhia.KidsLibrary.Application.Features.StoryViews.Commands;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class StoryViewsController(IMediator mediator) : ControllerBase
{
    [HttpPost("{id}/views")]
    public async Task<IActionResult> RegisterView(string id)
    {
        var (visitorKey, userId) = VisitorHelper.GetVisitorKey(HttpContext);

        await mediator.Send(new RegisterStoryViewCommand(id, visitorKey));

        return Ok(new { storyId = id, registered = true });
    }

}
