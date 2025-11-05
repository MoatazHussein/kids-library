using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.AddCustomStoryItem;
using Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.DeleteCustomStoryItem;
using Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.UpdateCustomStoryItem;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class CustomStoryItemsController(IMediator mediator) : ControllerBase
{
    [HttpPost("Add")]
    //[Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Add(AddCustomStoryItemCommand command)
    {
        string id = await mediator.Send(command);
        return StatusCode(201, $"Added successfully with Id {id}");
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateCustomStoryItemCommand command)
    {
        await mediator.Send(command);
        return StatusCode(200, $"Updated successfully");
    }

    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await mediator.Send(new DeleteCustomStoryItemCommand { Id = id });
        return StatusCode(200, $"Deleted successfully");
    }
}
