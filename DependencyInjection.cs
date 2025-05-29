using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Providers;
using Korrekturmanagementsystem.Providers.Interfaces;
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
        services.AddScoped<IRoleProvider, RoleProvider>();
        services.AddScoped<IMaterialProvider, MaterialProvider>();
        services.AddScoped<IReportProvider, ReportProvider>();
        services.AddScoped<IFileUploadProvider, FileUploadProvider>();
        services.AddScoped<IAttachmentProvider, AttachmentProvider>();
        services.AddScoped<IReportTagProvider, ReportTagProvider>();
        services.AddScoped<ICommentProvider, CommentProvider>();
        services.AddScoped<IReportHistoryProvider, ReportHistoryProvider>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IReportTagRepository, ReportTagRepository>();
        services.AddScoped<IReportHistoryRepository, ReportHistoryRepository>();
        services.AddScoped<ISystemRoleRepository, SystemRoleRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IMaterialTypeRepository, MaterialTypeRepository>();
        services.AddScoped<IPriorityRepository, PriorityRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IReportTypeRepository, ReportTypeRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
