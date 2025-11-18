using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salhia.KidsLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorNameToAllStories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterStories_AspNetUsers_CreatedBy",
                table: "MasterStories");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterStories_AspNetUsers_UpdatedBy",
                table: "MasterStories");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "MasterStories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "CustomStories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StoryViewSessions_UserId",
                table: "StoryViewSessions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterStories_AspNetUsers_CreatedBy",
                table: "MasterStories",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterStories_AspNetUsers_UpdatedBy",
                table: "MasterStories",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryViewSessions_AspNetUsers_UserId",
                table: "StoryViewSessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryViewSessions_MasterStories_MasterStoryId",
                table: "StoryViewSessions",
                column: "MasterStoryId",
                principalTable: "MasterStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterStories_AspNetUsers_CreatedBy",
                table: "MasterStories");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterStories_AspNetUsers_UpdatedBy",
                table: "MasterStories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryViewSessions_AspNetUsers_UserId",
                table: "StoryViewSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryViewSessions_MasterStories_MasterStoryId",
                table: "StoryViewSessions");

            migrationBuilder.DropIndex(
                name: "IX_StoryViewSessions_UserId",
                table: "StoryViewSessions");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "MasterStories");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "CustomStories");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterStories_AspNetUsers_CreatedBy",
                table: "MasterStories",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterStories_AspNetUsers_UpdatedBy",
                table: "MasterStories",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
