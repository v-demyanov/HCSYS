namespace HCSYS.Persistence.Entities;

public class PatientName : BaseEntity
{
    public string? Use { get; set; }

    required public string Family { get; set; }

    public string? Given { get; set; }

    public Guid PatientId { get; set; }

    public Patient? Patient { get; set; }
}
