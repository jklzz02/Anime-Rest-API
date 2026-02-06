using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVersionColumnToBanEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ban",
                table: "Ban");

            migrationBuilder.RenameTable(
                name: "Ban",
                newName: "ban");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "ban",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ban",
                table: "ban",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ban",
                table: "ban");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ban");

            migrationBuilder.RenameTable(
                name: "ban",
                newName: "Ban");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ban",
                table: "Ban",
                column: "Id");
        }
    }
}
