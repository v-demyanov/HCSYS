using HCSYS.PatientsService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCSYS.PatientsService.Persistence.Configurations;

public class PatientNameConfiguration : IEntityTypeConfiguration<PatientName>
{
    public void Configure(EntityTypeBuilder<PatientName> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Use)
               .IsRequired(false);

        builder.Property(x => x.Family)
               .IsRequired();

        builder.Property(x => x.Given)
               .IsRequired(false);
    }
}
