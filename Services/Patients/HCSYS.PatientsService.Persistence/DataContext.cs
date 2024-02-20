using HCSYS.PatientsService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCSYS.PatientsService.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<PatientName> PatientNames => this.Set<PatientName>();

    public DbSet<Patient> Patients => this.Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
