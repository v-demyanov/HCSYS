using HCSYS.Core.Models;

namespace HCSYS.ConsoleClient;

public interface IPatientsControllerClient
{
    Task<HttpResponseMessage> CreateAsync(CreatePatientRequest request);
}
