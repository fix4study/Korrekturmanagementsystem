using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Repositories;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services;
using Korrekturmanagementsystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplicationProviders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IRoleProvider, RoleProvider>();
        services.AddScoped<IMaterialProvider, MaterialProvider>();
        services.AddScoped<IReportProvider, ReportProvider>();
        services.AddScoped<IFileUploadProvider, FileUploadProvider>();
        services.AddScoped<IAttachmentProvider, AttachmentProvider>();
        services.AddScoped<IReportTagProvider, ReportTagProvider>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IReportTagRepository, ReportTagRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
