using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Repositories;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services;
using Korrekturmanagementsystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
