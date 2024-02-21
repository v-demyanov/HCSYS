using HCSYS.Api.Middlewares;
using HCSYS.Core;
using HCSYS.Persistence;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Patients API",
        Version = "v1",
    });

    Assembly entryAssembly = Assembly.GetExecutingAssembly();
    string xmlFilename = $"{entryAssembly.GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

    options.IncludeXmlComments(xmlPath);
});

builder.Services
    .AddPersistence(configuration.GetConnectionString("Default"))
    .AddCore();

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Channels API"));

if (!app.Environment.IsDevelopment())
{
    app.Services.MigrateDatabase();
}

app.MapControllers();

app.Run();
