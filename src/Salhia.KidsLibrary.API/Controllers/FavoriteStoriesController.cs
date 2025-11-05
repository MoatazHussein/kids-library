using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.AddFavoriteStories;
using Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.RemoveFavoriteStories;
using Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class FavoriteStoriesController(IMediator mediator) : ControllerBase
{
    [HttpPost("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromBody] GetFavoriteStoriesQuery query)
    {
        var favorites = await mediator.Send(query);
        return Ok(favorites);
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Add([FromBody] AddFavoriteStoriesCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("Remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove([FromBody] RemoveFavoriteStoriesCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
