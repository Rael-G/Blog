using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedTimePropertyNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "Posts",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "Comments",
                newName: "ModifiedTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "Posts",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "Comments",
                newName: "UpdateTime");
        }
    }
}
