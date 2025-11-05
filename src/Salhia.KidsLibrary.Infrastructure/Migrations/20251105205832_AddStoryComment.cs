using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoryComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    MasterStoryId = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryComments_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryComments_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryComments_MasterStories_MasterStoryId",
                        column: x => x.MasterStoryId,
                        principalTable: "MasterStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterStories_UpdatedBy",
                table: "MasterStories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StoryComments_CreatedAt",
                table: "StoryComments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StoryComments_CreatedBy",
                table: "StoryComments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StoryComments_Id",
                table: "StoryComments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StoryComments_MasterStoryId",
                table: "StoryComments",
                column: "MasterStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryComments_UpdatedBy",
                table: "StoryComments",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterStories_AspNetUsers_UpdatedBy",
                table: "MasterStories",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterStories_AspNetUsers_UpdatedBy",
                table: "MasterStories");

            migrationBuilder.DropTable(
                name: "StoryComments");

            migrationBuilder.DropIndex(
                name: "IX_MasterStories_UpdatedBy",
                table: "MasterStories");
        }
    }
}
