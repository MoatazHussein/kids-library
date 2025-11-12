using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterMasterStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "MasterStories",
                newName: "CoverImageUrl");

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "MasterStories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MediaUrl",
                table: "MasterStories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PublishYear",
                table: "MasterStories",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "MasterStories");

            migrationBuilder.DropColumn(
                name: "MediaUrl",
                table: "MasterStories");

            migrationBuilder.DropColumn(
                name: "PublishYear",
                table: "MasterStories");

            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "MasterStories",
                newName: "ImageUrl");

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    MasterStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItems_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaItems_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaItems_MasterStories_MasterStoryId",
                        column: x => x.MasterStoryId,
                        principalTable: "MasterStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_CreatedAt",
                table: "MediaItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_CreatedBy",
                table: "MediaItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_Id",
                table: "MediaItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MasterStoryId",
                table: "MediaItems",
                column: "MasterStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_Title",
                table: "MediaItems",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_UpdatedBy",
                table: "MediaItems",
                column: "UpdatedBy");
        }
    }
}
