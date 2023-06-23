using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Application.Validation.Extensions;

public static class ValidationExceptionExtensions
{
    public static ValidationProblemDetails ToProblemDetailes(this ValidationException ex)
    {
        var error = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = 400
        };
        
        var errors = ex.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                x => x.Key,
                x => x.Select(e => e.ErrorMessage).ToArray());

        foreach (var e in errors)
        {
            error.Errors[e.Key] = e.Value;
        }
        
        return error;
    }
}