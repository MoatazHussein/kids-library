namespace Salhia.KidsLibrary.Domain.Exceptions;

public class AppException : BaseException
{
    public AppException(string message, string? errorCode = null) 
        : base(message, errorCode ?? "InternalError", 500) 
    { }
}
