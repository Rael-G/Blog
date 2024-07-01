using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ArchiveIsPublic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Archive",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Archive",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Archive_OwnerId",
                table: "Archive",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Archive_Users_OwnerId",
                table: "Archive",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Archive_Users_OwnerId",
                table: "Archive");

            migrationBuilder.DropIndex(
                name: "IX_Archive_OwnerId",
                table: "Archive");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Archive");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Archive");
        }
    }
}
