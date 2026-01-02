namespace Salhia.KidsLibrary.Domain.Exceptions;

public class AlreadyExistsException : BaseException
{
    public AlreadyExistsException(string resourceType, string? errorCode = null)
        : base(resourceType + " Already Exists", errorCode ?? "AlreadyExists", 409)
    {
    }
}
