using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserAndMasterStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MasterStories_IsApproved",
                table: "MasterStories");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "MasterStories");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "MasterStories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_ApprovalStatus",
                table: "MasterStories",
                column: "ApprovalStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MasterStories_ApprovalStatus",
                table: "MasterStories");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "MasterStories");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "MasterStories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_IsApproved",
                table: "MasterStories",
                column: "IsApproved");
        }
    }
}
