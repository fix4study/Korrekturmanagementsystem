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
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connStr = Environment.GetEnvironmentVariable("CUSTOMCONNSTR__DefaultConnection");

        if (string.IsNullOrWhiteSpace(connStr))
        {
            connStr = configuration.GetConnectionString("DefaultConnection");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connStr));

        return services;
    }

}
