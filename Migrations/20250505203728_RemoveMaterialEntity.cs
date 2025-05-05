using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korrekturmanagementsystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaterialEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Materials_MaterialId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "Materials",
                schema: "kms");

            migrationBuilder.DropIndex(
                name: "IX_Reports_MaterialId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                schema: "kms",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialTypeId",
                schema: "kms",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CourseId",
                schema: "kms",
                table: "Reports",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MaterialTypeId",
                schema: "kms",
                table: "Reports",
                column: "MaterialTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Courses_CourseId",
                schema: "kms",
                table: "Reports",
                column: "CourseId",
                principalSchema: "kms",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_MaterialTypes_MaterialTypeId",
                schema: "kms",
                table: "Reports",
                column: "MaterialTypeId",
                principalSchema: "kms",
                principalTable: "MaterialTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Courses_CourseId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_MaterialTypes_MaterialTypeId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CourseId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_MaterialTypeId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CourseId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MaterialTypeId",
                schema: "kms",
                table: "Reports");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                schema: "kms",
                table: "Reports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Materials",
                schema: "kms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialTypeId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "kms",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalSchema: "kms",
                        principalTable: "MaterialTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MaterialId",
                schema: "kms",
                table: "Reports",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CourseId",
                schema: "kms",
                table: "Materials",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                schema: "kms",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Materials_MaterialId",
                schema: "kms",
                table: "Reports",
                column: "MaterialId",
                principalSchema: "kms",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
