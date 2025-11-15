using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryViewSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalViews",
                table: "MasterStoryStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StoryViewSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    MasterStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    VisitorKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    LastViewAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryViewSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoryViewSessions_LastViewAt",
                table: "StoryViewSessions",
                column: "LastViewAt");

            migrationBuilder.CreateIndex(
                name: "IX_StoryViewSessions_MasterStoryId_VisitorKey",
                table: "StoryViewSessions",
                columns: new[] { "MasterStoryId", "VisitorKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoryViewSessions");

            migrationBuilder.DropColumn(
                name: "TotalViews",
                table: "MasterStoryStats");
        }
    }
}
