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
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IAttachmentService, AttachmentService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
