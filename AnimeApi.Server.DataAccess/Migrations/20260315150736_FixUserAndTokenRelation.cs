using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixUserAndTokenRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "RefreshToken_User_Id_fk",
                table: "refresh_token");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_user_User_Id",
                table: "refresh_token",
                column: "User_Id",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_user_User_Id",
                table: "refresh_token");

            migrationBuilder.AddForeignKey(
                name: "RefreshToken_User_Id_fk",
                table: "refresh_token",
                column: "User_Id",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
