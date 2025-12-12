using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.SystemSettings.Commands.UpdateSystemSettings;
using Salhia.KidsLibrary.Application.Features.SystemSettings.Queries.GetSystemSettings;
using Salhia.KidsLibrary.Domain.Constants;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize(Roles = UserRoles.Admin)]
public class SystemSettingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var settings = await mediator.Send(new GetSystemSettingsQuery());
        return Ok(settings);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(UpdateSystemSettingsCommand command)
    {
        await mediator.Send(command);
        return StatusCode(200, $"Updated successfully");

    }
}
