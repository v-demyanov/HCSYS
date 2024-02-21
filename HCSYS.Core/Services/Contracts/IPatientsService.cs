using HCSYS.Core.Models;

namespace HCSYS.Core.Services.Contracts;

public interface IPatientsService
{
    Task<PatientDto> CreateAsync(CreatePatientRequest request);

    Task<PatientDto> GetByIdAsync(Guid patientId);

    Task UpdateAsync(UpdatePatientRequest request);

    Task DeleteAsync(Guid patientId);
}
