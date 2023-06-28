using Application.Validation.Extensions;
using FluentValidation;

namespace Application.Validation.Middleware;

public class ValidationMappingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            var error = ex.ToProblemDetails();
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}