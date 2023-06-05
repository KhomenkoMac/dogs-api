using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using codebridge.api.application.dogs_features.exceptions;
using codebridge.api.application.exceptions;

namespace codebridge.api.middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleException(e, context);
        }
        
    }

    private static Task HandleException(Exception exception, HttpContext context)
    {
        var responseCode = HttpStatusCode.InternalServerError;
        var responseMessage = string.Empty;
        
        switch (exception)
        {
            case ValidationException validationException:
                responseCode = HttpStatusCode.BadRequest;
                responseMessage = JsonSerializer.Serialize(validationException.Data);
                break;
            case NoSuchSortingAttributeException:
            case SuchNamedDogAlreadyExistException:
                responseCode = HttpStatusCode.Conflict;
                responseMessage = exception.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)responseCode;

        if (responseMessage == string.Empty)
        {
            responseMessage = JsonSerializer.Serialize(new { midlewareHandledErrorMessage = exception.Message });
        }

        return context.Response.WriteAsync(responseMessage);
    }
}
