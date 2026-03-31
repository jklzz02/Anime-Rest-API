using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "identity_provider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_provider", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_ProviderId",
                table: "user",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_identity_provider_ProviderId",
                table: "user");

            migrationBuilder.DropTable(
                name: "identity_provider");

            migrationBuilder.DropIndex(
                name: "IX_user_ProviderId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "user");
        }
    }
}
