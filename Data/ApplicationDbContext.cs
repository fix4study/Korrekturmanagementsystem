using Korrekturmanagementsystem.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; } = default!;
    public DbSet<ReportType> ReportTypes { get; set; } = default!;
    public DbSet<MaterialType> MaterialTypes { get; set; } = default!;
    public DbSet<Status> Statuses { get; set; } = default!;
    public DbSet<Priority> Priorities { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<ReportHistory> ReportHistories { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Attachment> Attachments { get; set; } = default!;
    public DbSet<SystemRole> SystemRoles { get; set; } = default!;
    public DbSet<StakeholderRole> StakeholderRoles { get; set; } = default!;
    public DbSet<ReportTag> ReportTags { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("kms");

        modelBuilder.Entity<ReportTag>()
            .HasKey(rt => new { rt.ReportId, rt.TagId });
    }

}
