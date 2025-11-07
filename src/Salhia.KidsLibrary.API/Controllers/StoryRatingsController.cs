using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.AddRating;
using Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.UpdateRating;
using Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.DeleteRating;
using Salhia.KidsLibrary.Application.Features.StoryRatings.Queries.GetRating;

namespace Salhia.KidsLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class StoryRatingsController(IMediator mediator) : ControllerBase
    {
        [HttpPost(nameof(GetRating))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetRating([FromBody] GetRatingQuery query)
        {
            var rating = await mediator.Send(query);
            
            if (rating == null)
                return NoContent();
            
            return Ok(rating);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add([FromBody] AddRatingCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateRatingCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteRatingCommand command)
        {
            await mediator.Send(command);
            return StatusCode(200, $"Deleted successfully");
        }
    }
}
