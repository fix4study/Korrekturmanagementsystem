using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korrekturmanagementsystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "kms");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tags",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Statuses",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "ReportTypes",
                newName: "ReportTypes",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "ReportTags",
                newName: "ReportTags",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Reports",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "ReportHistories",
                newName: "ReportHistories",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Priorities",
                newName: "Priorities",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "MaterialTypes",
                newName: "MaterialTypes",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Materials",
                newName: "Materials",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Courses",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comments",
                newSchema: "kms");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attachments",
                newSchema: "kms");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "kms",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "kms",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "kms",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                schema: "kms",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "kms",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "kms",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Statuses",
                schema: "kms",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "kms",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "ReportTypes",
                schema: "kms",
                newName: "ReportTypes");

            migrationBuilder.RenameTable(
                name: "ReportTags",
                schema: "kms",
                newName: "ReportTags");

            migrationBuilder.RenameTable(
                name: "Reports",
                schema: "kms",
                newName: "Reports");

            migrationBuilder.RenameTable(
                name: "ReportHistories",
                schema: "kms",
                newName: "ReportHistories");

            migrationBuilder.RenameTable(
                name: "Priorities",
                schema: "kms",
                newName: "Priorities");

            migrationBuilder.RenameTable(
                name: "MaterialTypes",
                schema: "kms",
                newName: "MaterialTypes");

            migrationBuilder.RenameTable(
                name: "Materials",
                schema: "kms",
                newName: "Materials");

            migrationBuilder.RenameTable(
                name: "Courses",
                schema: "kms",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "kms",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                schema: "kms",
                newName: "Attachments");
        }
    }
}
