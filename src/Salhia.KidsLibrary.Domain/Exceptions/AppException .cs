namespace Salhia.KidsLibrary.Domain.Exceptions;

public class AppException : BaseException
{
    public AppException(string message) : base(message, "InternalError", 500) { }

}
