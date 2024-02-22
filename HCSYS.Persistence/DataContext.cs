using HCSYS.Persistence.Entities;
using HCSYS.Persistence.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace HCSYS.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<PatientName> PatientNames => this.Set<PatientName>();

    public DbSet<Patient> Patients => this.Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();
    }
}
