using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Features.CustomStories.Commands.AddCustomStory;
using Salhia.KidsLibrary.Application.Features.CustomStories.Commands.DeleteCustomStory;
using Salhia.KidsLibrary.Application.Features.CustomStories.Commands.UpdateCustomStory;
using Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStories;
using Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStoryById;
using Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GenerateCustomStoryPdf;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class CustomStoriesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("GetById")]
    public async Task<IActionResult> GetById(GetCustomStoryByIdQuery query)
    {
        var customStory = await mediator.Send(query);
        return Ok(customStory);
    }

    [AllowAnonymous]
    [HttpPost("GetAllMatching")]
    public async Task<IActionResult> GetAllMatching([FromBody] GetCustomStoriesQuery query)
    {
        var customStories = await mediator.Send(query);
        return Ok(customStories);
    }

    [HttpPost("Add")]
    //[Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Add(AddCustomStoryCommand command)
    {
        string id = await mediator.Send(command);
        return StatusCode(201, $"Added successfully with Id {id}");
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateCustomStoryCommand command)
    {
        await mediator.Send(command);
        return StatusCode(200, $"Updated successfully");
    }

    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await mediator.Send(new DeleteCustomStoryCommand { Id = id });
        return StatusCode(200, $"Deleted successfully");
    }

    [HttpGet("DownloadPdf/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadPdf([FromRoute] string id)
    {
        var pdfBytes = await mediator.Send(new GenerateCustomStoryPdfQuery(id));

        return File(pdfBytes, "application/pdf", $"custom-story-{id}.pdf");
    }
}
