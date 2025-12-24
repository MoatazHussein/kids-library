namespace Salhia.KidsLibrary.Domain.Exceptions;
public class UnAuthorizedAccessException(string message)
    : BaseException(message, "Unauthorized", 401)
{
}

