using StandUsers.Domain.SharedKernel.Exceptions;
using StandUsers.Domain.User.Exceptions;
using System.Net;
using System.Text.Json;

namespace StandUsers.WebApi.Middlewares;

public class BusinessExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BusinessExceptionHandlerMiddleware> _logger;

    public BusinessExceptionHandlerMiddleware(RequestDelegate next, ILogger<BusinessExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
    {
        context.Response.ContentType = "application/json";
        int statusCode;
        string errorMessage;

        switch (ex)
        {
            case EmailAlreadyExists _:
                statusCode = (int)HttpStatusCode.Conflict;
                errorMessage = ex.Message;
                break;
            case IdentificationNumberAlreadyExists _:
                statusCode = (int)HttpStatusCode.Conflict;
                errorMessage = ex.Message;
                break;
            default:
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = "A business error occurred.";
                break;
        }

        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { message = errorMessage });
        return context.Response.WriteAsync(result);
    }

    private Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { detail = "An error occurred processing your request." });
        return context.Response.WriteAsync(result);
    }
}
