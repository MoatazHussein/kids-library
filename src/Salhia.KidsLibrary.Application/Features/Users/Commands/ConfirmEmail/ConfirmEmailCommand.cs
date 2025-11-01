using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<bool>
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}
