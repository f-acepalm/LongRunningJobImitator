using FluentValidation;
using LongRunningJobImitator.Api.Models;
using System.Net;
using System.Text.Json;

namespace LongRunningJobImitator.Api;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ValidationException ex)
        {
            await ConvertToBadRequest(context, ex);
        }
        catch (Exception ex)
        {
            await ConvertToServerErrorAsync(context, ex);
        }
    }

    private static Task ConvertToBadRequest(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var response = new ValidationErrorResponse(string.Join(';', ex.Errors.Select(x => x.ErrorMessage)));
        JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }

    private static Task ConvertToServerErrorAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = new ServerErrorResponse("An error occurred while processing your request. Please try again later.", ex.Message);

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }   
}
