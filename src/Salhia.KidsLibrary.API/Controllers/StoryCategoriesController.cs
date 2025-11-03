using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.AddStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.DeleteStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.UpdateStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;
using Salhia.KidsLibrary.Domain.Constants;

namespace Salhia.KidsLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class StoryCategoriesController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("GetAllMatching")]
        public async Task<IActionResult> GetAllMatching([FromBody] GetStoryCategoriesQuery query)
        {
            var storyCategories = await mediator.Send(query);
            return Ok(storyCategories);
        }

        [HttpPost("Add")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Add(AddStoryCategoryCommand command)
        {
            string id = await mediator.Send(command);
            return StatusCode(201, $"Added successfully with Id {id}");
        }

        [HttpPut("Update")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateStoryCategoryCommand command)
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
            await mediator.Send(new DeleteStoryCategoryCommand { Id = id });
            return StatusCode(200, $"Deleted successfully");
        }
    }
}
