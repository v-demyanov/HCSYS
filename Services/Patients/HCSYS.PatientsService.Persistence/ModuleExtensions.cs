using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HCSYS.PatientsService.Persistence;

public static class ModuleExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    public static void MigrateDatabase(this IServiceProvider provider)
    {
        DataContext dataContext = provider.GetRequiredService<DataContext>();
        dataContext.Database.Migrate();
    }
}
