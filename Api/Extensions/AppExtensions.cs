using Api.Middleware;
using Scalar.AspNetCore;

namespace Api.Extensions;

public static class AppExtensions
{
    public static WebApplication Middlewares(this WebApplication app)
    {
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }    

    public static WebApplication Scalar(this WebApplication app)
    {
        
        app.MapOpenApi();
        app.MapScalarApiReference();

        return app;
    }
}