using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class PostsPagination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedTime",
                table: "Posts",
                column: "CreatedTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_CreatedTime",
                table: "Posts");
        }
    }
}
