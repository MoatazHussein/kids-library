using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryCategory",
                table: "StoryCategory");

            migrationBuilder.RenameTable(
                name: "StoryCategory",
                newName: "StoryCategories");

            migrationBuilder.RenameIndex(
                name: "IX_StoryCategory_Title",
                table: "StoryCategories",
                newName: "IX_StoryCategories_Title");

            migrationBuilder.RenameIndex(
                name: "IX_StoryCategory_Id",
                table: "StoryCategories",
                newName: "IX_StoryCategories_Id");

            migrationBuilder.RenameIndex(
                name: "IX_StoryCategory_CreatedAt",
                table: "StoryCategories",
                newName: "IX_StoryCategories_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryCategories",
                table: "StoryCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MasterStories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    StoryCategoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterStories_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterStories_StoryCategories_StoryCategoryId",
                        column: x => x.StoryCategoryId,
                        principalTable: "StoryCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_CreatedAt",
                table: "MasterStories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_CreatedBy",
                table: "MasterStories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_Id",
                table: "MasterStories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_IsApproved",
                table: "MasterStories",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_StoryCategoryId",
                table: "MasterStories",
                column: "StoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_Title",
                table: "MasterStories",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterStories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryCategories",
                table: "StoryCategories");

            migrationBuilder.RenameTable(
                name: "StoryCategories",
                newName: "StoryCategory");

            migrationBuilder.RenameIndex(
                name: "IX_StoryCategories_Title",
                table: "StoryCategory",
                newName: "IX_StoryCategory_Title");

            migrationBuilder.RenameIndex(
                name: "IX_StoryCategories_Id",
                table: "StoryCategory",
                newName: "IX_StoryCategory_Id");

            migrationBuilder.RenameIndex(
                name: "IX_StoryCategories_CreatedAt",
                table: "StoryCategory",
                newName: "IX_StoryCategory_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryCategory",
                table: "StoryCategory",
                column: "Id");
        }
    }
}
