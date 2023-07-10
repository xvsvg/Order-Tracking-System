using Npgsql;

namespace Application.Middlewares;

public class DatabaseErrorMappingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is PostgresException or NpgsqlException)
        {
            context.Response.StatusCode = 503;
            await context.Response.WriteAsJsonAsync(ex.Data);
        }
    }
}