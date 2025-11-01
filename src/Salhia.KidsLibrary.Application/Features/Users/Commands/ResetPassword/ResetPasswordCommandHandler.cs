using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(IUserService userService) : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email;
            if (string.IsNullOrWhiteSpace(email))
                throw new NotFoundException("User", "email is empty");

            var token = Uri.UnescapeDataString(request.Token);
            var ok = await userService.ResetPasswordAsync(email, token, request.NewPassword, cancellationToken);

            if (!ok)
                throw new BusinessRuleException("Password reset failed", 400, "PASSWORD_RESET_FAILED");

            return true;
        }
    }
}
