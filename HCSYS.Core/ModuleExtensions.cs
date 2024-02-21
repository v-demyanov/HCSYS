using HCSYS.Core.Configuration;
using HCSYS.Core.Services;
using HCSYS.Core.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace HCSYS.Core;

public static class ModuleExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
        services.AddScoped<IPatientsService, PatientsService>();

        return services;
    }
}
