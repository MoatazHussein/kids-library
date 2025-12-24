using Microsoft.EntityFrameworkCore;
using Salhia.KidsLibrary.API.Models;
using Salhia.KidsLibrary.Domain.Exceptions;
using System.Text.Json;

namespace Salhia.KidsLibrary.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception ex,
        ILogger logger)
    {
        var (statusCode, message, errorCode) = ex switch
        {
            BaseException e => (e.StatusCode, e.Message, e.ErrorCode),
            DbUpdateException => (500, "Database update failed", "DatabaseError"),
            _ => (500, "Something went wrong", "InternalServerError")
        };

        context.Response.StatusCode = statusCode;

        var apiErrorResponse = new ApiErrorResponse
        {
            Code = errorCode,
            Message = message,
            StatusCode = statusCode
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(apiErrorResponse)
        );
    }
}
