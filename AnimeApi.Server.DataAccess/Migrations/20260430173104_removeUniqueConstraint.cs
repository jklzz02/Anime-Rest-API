using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserId_Ban",
                table: "ban");

            migrationBuilder.CreateIndex(
                name: "IX_ban_UserId",
                table: "ban",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ban_UserId",
                table: "ban");

            migrationBuilder.CreateIndex(
                name: "UserId_Ban",
                table: "ban",
                column: "UserId",
                unique: true);
        }
    }
}
