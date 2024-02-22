using HCSYS.ConsoleClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
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
    .Configure<GeneratorConfig>(configuration.GetRequiredSection(nameof(GeneratorConfig)))
    .AddSingleton<PatientsGenerator>()
    .BuildServiceProvider();

await serviceProvider
    .GetRequiredService<PatientsGenerator>()
    .RunAsync();
