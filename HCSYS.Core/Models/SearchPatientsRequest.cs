namespace HCSYS.Core.Models;

public record SearchPatientsRequest
{
    public required IEnumerable<string> BirthDateFilters { get; init; }
}
