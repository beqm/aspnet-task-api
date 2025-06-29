using Api.Middleware;
using Scalar.AspNetCore;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public static WebApplication AutoMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Database.Migrate();
        }

        return app;
    }
    

}