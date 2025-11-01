namespace Salhia.KidsLibrary.Domain.Exceptions;
public class UnAuthorizedAccessException(string message)
    : Exception(message)
{
}

