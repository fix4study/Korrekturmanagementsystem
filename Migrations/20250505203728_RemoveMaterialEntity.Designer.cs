﻿// <auto-generated />
using System;
using Korrekturmanagementsystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Korrekturmanagementsystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250505203728_RemoveMaterialEntity")]
    partial class RemoveMaterialEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("kms")
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Attachments", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ReportId");

                    b.ToTable("Comments", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Courses", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.MaterialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MaterialTypes", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Priority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Priorities", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("MaterialTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("PriorityId")
                        .HasColumnType("integer");

                    b.Property<int>("ReportTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("StatusId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("MaterialTypeId");

                    b.HasIndex("PriorityId");

                    b.HasIndex("ReportTypeId");

                    b.HasIndex("StatusId");

                    b.ToTable("Reports", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.ReportHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ChangedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<int>("StatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ChangedById");

                    b.HasIndex("ReportId");

                    b.HasIndex("StatusId");

                    b.ToTable("ReportHistories", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.ReportTag", b =>
                {
                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<int>("TagId")
                        .HasColumnType("integer");

                    b.HasKey("ReportId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ReportTags", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.ReportType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ReportTypes", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.StakeholderRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("StakeholderRoles", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Statuses", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.SystemRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SystemRoles", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StakeholderRoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SystemRoleId")
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("StakeholderRoleId");

                    b.HasIndex("SystemRoleId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", "kms");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Attachment", b =>
                {
                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Report", "Report")
                        .WithMany("Attachments")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Comment", b =>
                {
                    b.HasOne("Korrekturmanagementsystem.Data.Entities.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Report", "Report")
                        .WithMany("Comments")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Report", b =>
                {
                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Course", "Course")
                        .WithMany("Reports")
                        .HasForeignKey("CourseId");

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.User", "CreatedBy")
                        .WithMany("CreatedReports")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.MaterialType", "MaterialType")
                        .WithMany("Reports")
                        .HasForeignKey("MaterialTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Priority", "Priority")
                        .WithMany()
                        .HasForeignKey("PriorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.ReportType", "ReportType")
                        .WithMany("Reports")
                        .HasForeignKey("ReportTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("CreatedBy");

                    b.Navigation("MaterialType");

                    b.Navigation("Priority");

                    b.Navigation("ReportType");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.ReportHistory", b =>
                {
                    b.HasOne("Korrekturmanagementsystem.Data.Entities.User", "ChangedBy")
                        .WithMany("ChangedReports")
                        .HasForeignKey("ChangedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Report", "Report")
                        .WithMany("History")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChangedBy");

                    b.Navigation("Report");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.ReportTag", b =>
                {
                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Report", "Report")
                        .WithMany("ReportTags")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.Tag", "Tag")
                        .WithMany("ReportTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.User", b =>
                {
                    b.HasOne("Korrekturmanagementsystem.Data.Entities.StakeholderRole", "StakeholderRole")
                        .WithMany("Users")
                        .HasForeignKey("StakeholderRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Korrekturmanagementsystem.Data.Entities.SystemRole", "SystemRole")
                        .WithMany("Users")
                        .HasForeignKey("SystemRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StakeholderRole");

                    b.Navigation("SystemRole");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Course", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.MaterialType", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Report", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Comments");

                    b.Navigation("History");

                    b.Navigation("ReportTags");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.ReportType", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.StakeholderRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.SystemRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.Tag", b =>
                {
                    b.Navigation("ReportTags");
                });

            modelBuilder.Entity("Korrekturmanagementsystem.Data.Entities.User", b =>
                {
                    b.Navigation("ChangedReports");

                    b.Navigation("Comments");

                    b.Navigation("CreatedReports");
                });
#pragma warning restore 612, 618
        }
    }
}
