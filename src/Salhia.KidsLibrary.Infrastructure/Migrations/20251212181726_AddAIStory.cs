using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAIStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryLikes_AspNetUsers_UserId",
                table: "StoryLikes");

            migrationBuilder.CreateTable(
                name: "AIStories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    StoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HeroName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HeroImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SlidesCount = table.Column<int>(type: "int", nullable: false),
                    CustomStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIStories_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AIStories_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AIStories_CustomStories_CustomStoryId",
                        column: x => x.CustomStoryId,
                        principalTable: "CustomStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AIStorySlides",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImagePrompt = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AIStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIStorySlides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIStorySlides_AIStories_AIStoryId",
                        column: x => x.AIStoryId,
                        principalTable: "AIStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AIStorySlides_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AIStorySlides_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIStories_CreatedBy",
                table: "AIStories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AIStories_CustomStoryId",
                table: "AIStories",
                column: "CustomStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AIStories_UpdatedBy",
                table: "AIStories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AIStorySlides_AIStoryId",
                table: "AIStorySlides",
                column: "AIStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AIStorySlides_CreatedBy",
                table: "AIStorySlides",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AIStorySlides_Status",
                table: "AIStorySlides",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AIStorySlides_UpdatedBy",
                table: "AIStorySlides",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryLikes_AspNetUsers_UserId",
                table: "StoryLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryLikes_AspNetUsers_UserId",
                table: "StoryLikes");

            migrationBuilder.DropTable(
                name: "AIStorySlides");

            migrationBuilder.DropTable(
                name: "AIStories");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryLikes_AspNetUsers_UserId",
                table: "StoryLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
