using HCSYS.Persistence.Enums;

namespace HCSYS.Core.Models;

public record UpdatePatientRequest
{
    public Guid PatientToUpdateId { get; init; }

    public string? Use { get; init; }

    required public string Family { get; init; }

    public string? Given { get; init; }

    public Gender Gender { get; init; }

    public DateTimeOffset BirthDate { get; init; }

    public bool IsActive { get; init; }
}
