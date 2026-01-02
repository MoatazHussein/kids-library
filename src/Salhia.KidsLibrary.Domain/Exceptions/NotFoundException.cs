namespace Salhia.KidsLibrary.Domain.Exceptions;
public class NotFoundException : BaseException
{
    public NotFoundException(string resourceType, string resourceIdentifier, string? errorCode = null)
        : base($"{resourceType} with : {resourceIdentifier} doesn't exist", errorCode ?? "NotFound", 404)
    {
    }
}
