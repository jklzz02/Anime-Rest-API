using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityProviderFkRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "user",
                newName: "Provider_Id");

            migrationBuilder.RenameIndex(
                name: "IX_user_ProviderId",
                table: "user",
                newName: "IX_user_Provider_Id");

            migrationBuilder.AlterColumn<int>(
                name: "Provider_Id",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_user_identity_provider_Provider_Id",
                table: "user",
                column: "Provider_Id",
                principalTable: "identity_provider",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_identity_provider_Provider_Id",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "Provider_Id",
                table: "user",
                newName: "ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_user_Provider_Id",
                table: "user",
                newName: "IX_user_ProviderId");

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "user",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);
        }
    }
}
