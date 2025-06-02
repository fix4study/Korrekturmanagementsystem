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
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IReportTagService, ReportTagService>();
        services.AddScoped<IReportHistoryService, ReportHistoryService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IReportOptionsService, ReportOptionsService>();

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
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IStakeholderRoleRepository, StakeholderRoleRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
