using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Infrastructure.Repositories;
using Gymly.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gymly.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<GymlyDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Gymly.Infrastructure")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<GymlyDbContext>());

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IQrCodeService, QrCodeService>();

        return services;
    }
}