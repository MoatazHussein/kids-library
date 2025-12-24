namespace Salhia.KidsLibrary.Domain.Exceptions;

public class AlreadyExistsException(string resourceType)
    : BaseException(resourceType + " Already Exists", "AlreadyExists", 409)
{
}
