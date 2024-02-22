
using HCSYS.Core.Models;
using HCSYS.Persistence.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HCSYS.ConsoleClient;

public class PatientsGenerator
{
    private readonly IPatientsControllerClient _patientsControllerClient;
    private readonly GeneratorConfig _generatorConfig;
    private readonly ILogger<PatientsGenerator> _logger;

    public PatientsGenerator(
        IPatientsControllerClient patientsControllerClient,
        IOptions<GeneratorConfig> options,
        ILogger<PatientsGenerator> logger)
    {
        _patientsControllerClient = patientsControllerClient ?? throw new ArgumentNullException(nameof(patientsControllerClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _generatorConfig = options.Value;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Running generating of patients...");

        var requestCount = 1;
        var createdCount = 0;
        while (requestCount <= _generatorConfig.PatientsCount)
        {
            try
            {
                HttpResponseMessage response = await _patientsControllerClient
                    .CreateAsync(new CreatePatientRequest
                    {
                        Use = $"Request {requestCount}",
                        Family = $"Request {requestCount}",
                        Given = $"Request {requestCount}",
                        Gender = Gender.Male,
                        BirthDate = DateTimeOffset.UtcNow,
                        IsActive = true,
                    });

                response.EnsureSuccessStatusCode();
                createdCount++;
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError($"Error while creating new patient: {exception}");
            }

            requestCount++;
        }

        _logger.LogInformation("Generating of patients has been finished.");
        _logger.LogInformation($"{createdCount} patient's has been inserted.");
    }
}
