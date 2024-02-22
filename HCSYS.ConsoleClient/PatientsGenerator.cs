
using HCSYS.Core.Models;
using HCSYS.Persistence.Enums;
using Microsoft.Extensions.Options;

namespace HCSYS.ConsoleClient;

public class PatientsGenerator
{
    private readonly IPatientsControllerClient _patientsControllerClient;
    private readonly GeneratorConfig _generatorConfig;

    public PatientsGenerator(IPatientsControllerClient patientsControllerClient, IOptions<GeneratorConfig> options)
    {
        _patientsControllerClient = patientsControllerClient ?? throw new ArgumentNullException(nameof(patientsControllerClient));
        _generatorConfig = options.Value;
    }

    public async Task RunAsync()
    {
        var requestCount = 1;
        while (requestCount <= _generatorConfig.PatientsCount)
        {
            await _patientsControllerClient.CreateAsync(new CreatePatientRequest
            {
                Use = $"Request {requestCount}",
                Family = $"Request {requestCount}",
                Given = $"Request {requestCount}",
                Gender = Gender.Male,
                BirthDate = DateTimeOffset.UtcNow,
                IsActive = true,
            });

            requestCount++;
        }
    }
}
