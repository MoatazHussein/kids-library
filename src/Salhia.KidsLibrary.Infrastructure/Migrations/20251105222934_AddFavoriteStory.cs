using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriteStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaItems_MasterStories_MasterStoryId1",
                table: "MediaItems");

            migrationBuilder.DropIndex(
                name: "IX_MediaItems_MasterStoryId1",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "MasterStoryId1",
                table: "MediaItems");

            migrationBuilder.CreateTable(
                name: "FavoriteStories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    MasterStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteStories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteStories_MasterStories_MasterStoryId",
                        column: x => x.MasterStoryId,
                        principalTable: "MasterStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_CreatedAt",
                table: "FavoriteStories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_Id",
                table: "FavoriteStories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_MasterStoryId",
                table: "FavoriteStories",
                column: "MasterStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_UserId",
                table: "FavoriteStories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_UserId_MasterStoryId",
                table: "FavoriteStories",
                columns: new[] { "UserId", "MasterStoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteStories");

            migrationBuilder.AddColumn<string>(
                name: "MasterStoryId1",
                table: "MediaItems",
                type: "nvarchar(26)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MasterStoryId1",
                table: "MediaItems",
                column: "MasterStoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaItems_MasterStories_MasterStoryId1",
                table: "MediaItems",
                column: "MasterStoryId1",
                principalTable: "MasterStories",
                principalColumn: "Id");
        }
    }
}
