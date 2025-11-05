using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.DeleteStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;
using Salhia.KidsLibrary.Domain.Constants;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class StoryCommentsController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("GetAllMatching")]
    public async Task<IActionResult> GetAllMatching([FromBody] GetStoryCommentsQuery query)
    {
        var storyComments = await mediator.Send(query);
        return Ok(storyComments);
    }

    [HttpPost("Add")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Add(AddStoryCommentCommand command)
    {
        string id = await mediator.Send(command);
        return StatusCode(201, $"Added successfully with Id {id}");
    }

    [HttpPut("Update")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateStoryCommentCommand command)
    {
        await mediator.Send(command);
        return StatusCode(200, $"Updated successfully");
    }

    [HttpDelete("Delete/{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await mediator.Send(new DeleteStoryCommentCommand { Id = id });
        return StatusCode(200, $"Deleted successfully");
    }
}
