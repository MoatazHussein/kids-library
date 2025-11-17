using Salhia.KidsLibrary.Application.Features.Users.Commands.AssignUserRole;
using Salhia.KidsLibrary.Application.Features.Users.Commands.ChangeUserRole;
using Salhia.KidsLibrary.Application.Features.Users.Commands.ConfirmEmail;
using Salhia.KidsLibrary.Application.Features.Users.Commands.DisableUser;
using Salhia.KidsLibrary.Application.Features.Users.Commands.EnableUser;
using Salhia.KidsLibrary.Application.Features.Users.Commands.ForgotPassword;
using Salhia.KidsLibrary.Application.Features.Users.Commands.Login;
using Salhia.KidsLibrary.Application.Features.Users.Commands.ResetPassword;
using Salhia.KidsLibrary.Application.Features.Users.Commands.UnassignUserRole;
using Salhia.KidsLibrary.Application.Features.Users.Queries.GetAllUsers;
using Salhia.KidsLibrary.Application.Features.Users.Queries.GetCurrentUser;
using Salhia.KidsLibrary.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.Users.Commands.RegisterUser;
using Salhia.KidsLibrary.Application.Features.Users.Commands.UpdateUser;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IMediator mediator, IConfiguration configuration) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> RegisterClient(RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Ok("User registered successfully.");
    }

    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        await mediator.Send(command);
        return Ok("User updated successfully.");
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost("users")]
    public async Task<IActionResult> GetUsers(GetAllUsersQuery command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("users/me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await mediator.Send(new GetCurrentUserQuery(User));
        return Ok(result);
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost("userRole")]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut("userRole/change")]
    public async Task<IActionResult> ChangeUserRole(ChangeUserRoleCommand command)
    {
        await mediator.Send(command);
        return Ok(new { message = $"User role changed successfully to {command.NewRole}" });
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("userRole")]
    public async Task<IActionResult> UnassignedUserRole(UnassignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Disable a user account (user will be logged out on next request)
    /// </summary>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut("users/{userId}/disable")]
    public async Task<IActionResult> DisableUser([FromRoute] string userId)
    {
        await mediator.Send(new DisableUserCommand { UserId = userId });
        return Ok(new { message = "User has been disabled successfully" });
    }

    /// <summary>
    /// Enable a previously disabled user account
    /// </summary>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut("users/{userId}/enable")]
    public async Task<IActionResult> EnableUser([FromRoute] string userId)
    {
        await mediator.Send(new EnableUserCommand { UserId = userId });
        return Ok(new { message = "User has been enabled successfully" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await mediator.Send(command);
        return Ok("If an account exists with that email, a reset link was sent.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var success = await mediator.Send(command);
        if (!success)
            return BadRequest("Invalid token or user not found.");

        return Ok("Password has been reset successfully.");
    }

    [HttpGet("reset-password")]
    public IActionResult ShowResetPasswordPage([FromQuery] string email, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return BadRequest("Email confirmation failed.");


        return Redirect($"{configuration["App:FrontendBaseUrl"]}/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
    }



    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return BadRequest("Invalid email or token.");

        var command = new ConfirmEmailCommand
        {
            Email = email,
            Token = token
        };

        var result = await mediator.Send(command);

        if (!result)
            return BadRequest("Email confirmation failed.");

        //return Ok("Email confirmed successfully.");
        return Redirect($"{configuration["App:FrontendBaseUrl"]}/successful_verification?email={email}&token={token}");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

}