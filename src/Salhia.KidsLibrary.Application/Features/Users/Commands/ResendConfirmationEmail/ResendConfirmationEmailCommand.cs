using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ResendConfirmationEmail;

public record ResendConfirmationEmailCommand(string Email) : IRequest<bool>;
