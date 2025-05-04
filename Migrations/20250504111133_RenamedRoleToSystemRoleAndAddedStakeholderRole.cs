using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korrekturmanagementsystem.Migrations
{
    /// <inheritdoc />
    public partial class RenamedRoleToSystemRoleAndAddedStakeholderRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                schema: "kms",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "kms");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "kms",
                table: "Users",
                newName: "SystemRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                schema: "kms",
                table: "Users",
                newName: "IX_Users_SystemRoleId");

            migrationBuilder.AddColumn<Guid>(
                name: "StakeholderRoleId",
                schema: "kms",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StakeholderRoles",
                schema: "kms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakeholderRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                schema: "kms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_StakeholderRoleId",
                schema: "kms",
                table: "Users",
                column: "StakeholderRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StakeholderRoles_StakeholderRoleId",
                schema: "kms",
                table: "Users",
                column: "StakeholderRoleId",
                principalSchema: "kms",
                principalTable: "StakeholderRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SystemRoles_SystemRoleId",
                schema: "kms",
                table: "Users",
                column: "SystemRoleId",
                principalSchema: "kms",
                principalTable: "SystemRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_StakeholderRoles_StakeholderRoleId",
                schema: "kms",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SystemRoles_SystemRoleId",
                schema: "kms",
                table: "Users");

            migrationBuilder.DropTable(
                name: "StakeholderRoles",
                schema: "kms");

            migrationBuilder.DropTable(
                name: "SystemRoles",
                schema: "kms");

            migrationBuilder.DropIndex(
                name: "IX_Users_StakeholderRoleId",
                schema: "kms",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StakeholderRoleId",
                schema: "kms",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SystemRoleId",
                schema: "kms",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SystemRoleId",
                schema: "kms",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "kms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                schema: "kms",
                table: "Users",
                column: "RoleId",
                principalSchema: "kms",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
