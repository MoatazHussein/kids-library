using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IUserService userService,
    IMailService mailService,
    IConfiguration config) : IRequestHandler<ForgotPasswordCommand, bool>
{
    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null || !await userService.IsEmailConfirmedAsync(request.Email, cancellationToken))
        {
            throw new NotFoundException("User", request.Email);
        }

        var token = await userService.GeneratePasswordResetTokenAsync(user.Id, cancellationToken);

        if (user.Email is not null && token is not null)
        {

            var resetUrl = $"{config["App:ResetPasswordUrl"]}?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";

            var emailBody = $@"
            <p>Hi,</p>
            <p>You requested to reset your password. Click the link below:</p>
            <p><a href='{resetUrl}'>Reset your password</a></p>
            <p>If you didn’t request this, just ignore this email.</p>";

            await mailService.SendEmailAsync(user.Email!, "Password Reset Request", emailBody, null);

        }


        return true;
    }
}
