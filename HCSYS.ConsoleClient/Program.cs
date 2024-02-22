using HCSYS.ConsoleClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.Development.json", true)
    .AddEnvironmentVariables()
    .Build();

var serviceCollection = new ServiceCollection();
serviceCollection.AddHttpClient<IPatientsControllerClient, PatientsControllerClient>(client =>
{
    string? baseApiUri = configuration.GetValue<string>("BaseApiUri");
    if (baseApiUri is null)
    {
        throw new NullReferenceException(nameof(baseApiUri));
    }

    client.BaseAddress = new Uri(baseApiUri);
});

ServiceProvider serviceProvider = serviceCollection
    .AddLogging(builder =>
    {
        Logger logger = ConfigureLogging();
        builder.AddSerilog(logger);
    })
    .Configure<GeneratorConfig>(configuration.GetRequiredSection(nameof(GeneratorConfig)))
    .AddSingleton<PatientsGenerator>()
    .BuildServiceProvider();

await serviceProvider.GetRequiredService<PatientsGenerator>()
    .RunAsync();

static Logger ConfigureLogging()
{
    const string template =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    return new LoggerConfiguration()
        .WriteTo.Console(outputTemplate: template, theme: AnsiConsoleTheme.Code)
        .CreateLogger();
}