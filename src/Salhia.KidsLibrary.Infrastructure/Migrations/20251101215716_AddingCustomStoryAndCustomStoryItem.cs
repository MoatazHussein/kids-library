using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingCustomStoryAndCustomStoryItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomStories",
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
                    table.PrimaryKey("PK_CustomStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomStories_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomStories_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomStoryItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustomStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomStoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomStoryItems_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomStoryItems_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomStoryItems_CustomStories_CustomStoryId",
                        column: x => x.CustomStoryId,
                        principalTable: "CustomStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomStories_CreatedAt",
                table: "CustomStories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStories_CreatedBy",
                table: "CustomStories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStories_Id",
                table: "CustomStories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStories_Title",
                table: "CustomStories",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStories_UpdatedBy",
                table: "CustomStories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStoryItems_CreatedAt",
                table: "CustomStoryItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStoryItems_CreatedBy",
                table: "CustomStoryItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStoryItems_CustomStoryId",
                table: "CustomStoryItems",
                column: "CustomStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStoryItems_Id",
                table: "CustomStoryItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStoryItems_Title",
                table: "CustomStoryItems",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStoryItems_UpdatedBy",
                table: "CustomStoryItems",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomStoryItems");

            migrationBuilder.DropTable(
                name: "CustomStories");
        }
    }
}
