namespace Salhia.KidsLibrary.Domain.Exceptions;
public class NotFoundException(string resourceType, string resourceIdentifier)
    : BaseException($"{resourceType} with : {resourceIdentifier} doesn't exist", "NotFound", 404)
{
}
