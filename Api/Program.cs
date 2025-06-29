using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logger(builder.Configuration)
    .Database(builder.Configuration)
    .Repositories()
    .Services()
    .Swagger()
    .Authentication()
    .AutoMapper()
    .Versioning()
    .Mediatr();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.Scalar();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Middlewares();
app.AutoMigrations();
app.Metrics();

app.Run();