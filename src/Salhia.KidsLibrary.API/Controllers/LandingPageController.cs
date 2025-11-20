using MediatR;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.LandingPage.Queries.GetLandingPageStats;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LandingPageController(IMediator mediator) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await mediator.Send(new GetLandingPageStatsQuery());
        return Ok(result);
    }
}
