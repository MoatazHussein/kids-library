using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.AddMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.DeleteMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.UpdateMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;

namespace Salhia.KidsLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class MediaItemsController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("GetAllMatching")]
        public async Task<IActionResult> GetAllMatching([FromBody] GetMediaItemsQuery query)
        {
            var mediaItems = await mediator.Send(query);
            return Ok(mediaItems);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddMediaItemCommand command)
        {
            string id = await mediator.Send(command);
            return StatusCode(201, $"Added successfully with Id {id}");
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateMediaItemCommand command)
        {
            await mediator.Send(command);
            return StatusCode(200, $"Updated successfully");
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await mediator.Send(new DeleteMediaItemCommand { Id = id });
            return StatusCode(200, $"Deleted successfully");
        }
    }
}
