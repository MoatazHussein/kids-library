using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingAndStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterStoryStats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    MasterStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    RatingsCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RatingsSum = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterStoryStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterStoryStats_MasterStories_MasterStoryId",
                        column: x => x.MasterStoryId,
                        principalTable: "MasterStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryRatings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    MasterStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryRatings_MasterStories_MasterStoryId",
                        column: x => x.MasterStoryId,
                        principalTable: "MasterStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterStoryStats_MasterStoryId",
                table: "MasterStoryStats",
                column: "MasterStoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoryRatings_MasterStoryId",
                table: "StoryRatings",
                column: "MasterStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryRatings_UserId",
                table: "StoryRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryRatings_UserId_MasterStoryId",
                table: "StoryRatings",
                columns: new[] { "UserId", "MasterStoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterStoryStats");

            migrationBuilder.DropTable(
                name: "StoryRatings");
        }
    }
}
