using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<bool>
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
