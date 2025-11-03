using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoryCategory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoryCategory_CreatedAt",
                table: "StoryCategory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StoryCategory_Id",
                table: "StoryCategory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StoryCategory_Title",
                table: "StoryCategory",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoryCategory");
        }
    }
}
