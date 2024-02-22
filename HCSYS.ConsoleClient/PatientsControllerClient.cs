using HCSYS.Core.Models;
using System.Net.Http.Json;

namespace HCSYS.ConsoleClient;

public class PatientsControllerClient : IPatientsControllerClient
{
    public PatientsControllerClient(HttpClient httpClient)
    {
        this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task CreateAsync(CreatePatientRequest request) =>
        await HttpClient.PostAsJsonAsync(BaseRoute, request);

    protected HttpClient HttpClient { get; }

    protected string BaseRoute { get; } = "/api/patients";
}
