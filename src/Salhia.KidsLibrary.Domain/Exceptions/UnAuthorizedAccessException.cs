namespace Salhia.KidsLibrary.Domain.Exceptions;
public class UnAuthorizedAccessException : BaseException
{
    public UnAuthorizedAccessException(string message, string? errorCode = null)
        : base(message, errorCode ?? "Unauthorized", 401)
    {
    }
}

