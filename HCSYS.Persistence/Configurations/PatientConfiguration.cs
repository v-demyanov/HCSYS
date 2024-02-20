using HCSYS.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCSYS.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Name)
               .WithOne(x => x.Patient)
               .HasForeignKey<PatientName>(x => x.PatientId)
               .IsRequired();

        builder.Property(x => x.BirthDate)
               .IsRequired();
    }
}
