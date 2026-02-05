using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeBanEntityUserIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ban_UserId",
                table: "Ban");

            migrationBuilder.CreateIndex(
                name: "UserId_Ban",
                table: "Ban",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserId_Ban",
                table: "Ban");

            migrationBuilder.CreateIndex(
                name: "IX_Ban_UserId",
                table: "Ban",
                column: "UserId");
        }
    }
}
