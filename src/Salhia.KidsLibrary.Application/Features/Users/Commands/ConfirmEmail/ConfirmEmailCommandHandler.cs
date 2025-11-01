using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;
using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler(IUserService userService) : IRequestHandler<ConfirmEmailCommand, bool>
{
    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByEmailAsync(request.Email, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(AppUser), request.Email.ToString());

        if (user.EmailConfirmed)
        {
            throw new BusinessRuleException("Email already confirmed", 200, "EMAIL_ALREADY_CONFIRMED");
        }

        var result = await userService.ConfirmEmailAsync(user.Id, request.Token, cancellationToken);


        return result;

    }
}
