using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    IUserService userService,
    IMailService mailService,
    IMapper mapper,
    IConfiguration config
    ) : IRequestHandler<RegisterUserCommand, string>
{
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerUserRequest = mapper.Map<RegisterUserRequest>(request);

        var user = await userService.CreateUserAsync(registerUserRequest, cancellationToken);

        await userService.AddToRoleAsync(user.Id, request.UserType.ToString() , cancellationToken);

        var token = await userService.GenerateEmailConfirmationTokenAsync(user.Id, cancellationToken);

        if (user.Email is null || token is null)
            throw new BusinessRuleException("Error generating email confirmation token", 500, "EMAIL_CONFIRMATION_TOKEN_ERROR");

        var confirmUrl = $"{config["App:ConfirmEmailApiUrl"]}?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token!)}";

        var emailBody = $"<p>Please confirm your email by clicking <a href='{confirmUrl}'>here</a>.</p>";

        await mailService.SendEmailAsync(user.Email, "Confirm your email", emailBody, null);


        return user.Id;
    }
}
