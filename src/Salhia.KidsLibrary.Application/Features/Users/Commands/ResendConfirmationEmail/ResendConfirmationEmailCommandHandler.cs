using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandHandler(
    IUserService userService,
    IMailService mailService,
    IConfiguration config) : IRequestHandler<ResendConfirmationEmailCommand, bool>
{
    public async Task<bool> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User", request.Email, "USER_NOT_FOUND");
        }

        if (await userService.IsEmailConfirmedAsync(request.Email, cancellationToken))
        {
            throw new BusinessRuleException("Email is already confirmed", 400, "EMAIL_ALREADY_CONFIRMED");
        }

        // Generate a new confirmation token
        var token = await userService.GenerateEmailConfirmationTokenAsync(user.Id, cancellationToken);

        if (token is null)
        {
            throw new BusinessRuleException("Error generating email confirmation token", 500, "EMAIL_CONFIRMATION_TOKEN_ERROR");
        }

        var confirmUrl = $"{config["App:ConfirmEmailApiUrl"]}?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";

        var emailBody = $@"
            <p>Hi,</p>
            <p>You requested a new email confirmation link.</p>
            <p>Please confirm your email by clicking <a href='{confirmUrl}'>here</a>.</p>
            <p>If you didn't request this, you can safely ignore this email.</p>";

        await mailService.SendEmailAsync(user.Email!, "Confirm your email", emailBody, null);

        return true;
    }
}
