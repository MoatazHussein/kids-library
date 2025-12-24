namespace Salhia.KidsLibrary.API.Models;

public sealed class ApiErrorResponse
{
    public string Code { get; init; } = default!;
    public string Message { get; init; } = default!;
    public int StatusCode { get; init; }
}


