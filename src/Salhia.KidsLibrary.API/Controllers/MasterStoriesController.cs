using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStory;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStoryWithItems;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.ApproveMasterStory;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.DeleteMasterStory;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.UpdateMasterStory;
using Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;
using Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class MasterStoriesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("GetAllMatching")]
    public async Task<IActionResult> GetAllMatching([FromBody] GetMasterStoriesQuery query)
    {
        var masterStories = await mediator.Send(query);
        return Ok(masterStories);
    }

    [AllowAnonymous]
    [HttpPost("GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromBody] GetMasterStoryByIdQuery query)
    {
        var masterStory = await mediator.Send(query);
        return Ok(masterStory);
    }

    [HttpPost("Add")]
    //[Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Add(AddMasterStoryCommand command)
    {
        string id = await mediator.Send(command);
        return StatusCode(201, $"Added successfully with Id {id}");
    }

    [HttpPost("AddWithItems")]
    //[Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddWithItems(AddMasterStoryWithItemsCommand command)
    {
        string id = await mediator.Send(command);
        return StatusCode(201, new { Id = id, Message = "Master story with items added successfully" });
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateMasterStoryCommand command)
    {
        await mediator.Send(command);
        return StatusCode(200, $"Updated successfully");
    }

    [HttpPatch("Approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Approve(ApproveMasterStoryCommand command)
    {
        await mediator.Send(command);
        return StatusCode(200, $"Approval status updated successfully");
    }

    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await mediator.Send(new DeleteMasterStoryCommand { Id = id });
        return StatusCode(200, $"Deleted successfully");
    }
}
