using HCSYS.Core.Models;

namespace HCSYS.ConsoleClient;

public interface IPatientsControllerClient
{
    Task CreateAsync(CreatePatientRequest request);
}
