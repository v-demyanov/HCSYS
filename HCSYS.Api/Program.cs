using HCSYS.Api.Middlewares;
using HCSYS.Core;
using HCSYS.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddPersistence(configuration.GetConnectionString("Default"))
    .AddCore();

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.Services.MigrateDatabase();
}

app.Run();
