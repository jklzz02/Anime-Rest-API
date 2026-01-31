using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimeApi.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "licensor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "producer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Access = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "source",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestampz", nullable: false),
                    Picture_Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "User_Role_Id_fk",
                        column: x => x.Role_Id,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    English_Name = table.Column<string>(type: "text", nullable: false),
                    Other_Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Synopsis = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Image_URL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Episodes = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Release_Year = table.Column<int>(type: "integer", nullable: false),
                    Started_Airing = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Finished_Airing = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Rating = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Studio = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Score = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: true),
                    Trailer_image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Trailer_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Trailer_embed_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Background = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "Anime_Source_Id_fk",
                        column: x => x.SourceId,
                        principalTable: "source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Anime_Type_Id_fk",
                        column: x => x.TypeId,
                        principalTable: "type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hashed_Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestampz", nullable: false),
                    Expires_At = table.Column<DateTime>(type: "timestampz", nullable: false),
                    Revoked_At = table.Column<DateTime>(type: "timestampz", nullable: true),
                    User_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "RefreshToken_User_Id_fk",
                        column: x => x.User_Id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anime_genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "Anime_Genre_ibfk_1",
                        column: x => x.AnimeId,
                        principalTable: "anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Anime_Genre_ibfk_2",
                        column: x => x.GenreId,
                        principalTable: "genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anime_licensor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    LicensorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "Anime_Licensor_ibfk_1",
                        column: x => x.AnimeId,
                        principalTable: "anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Anime_Licensor_ibfk_2",
                        column: x => x.LicensorId,
                        principalTable: "licensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anime_producer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    ProducerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "Anime_Producer_ibfk_1",
                        column: x => x.AnimeId,
                        principalTable: "anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Anime_Producer_ibfk_2",
                        column: x => x.ProducerId,
                        principalTable: "producer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Anime_Id = table.Column<int>(type: "integer", nullable: false),
                    User_Id = table.Column<int>(type: "integer", nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "Anime_Id_fk",
                        column: x => x.Anime_Id,
                        principalTable: "anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "User_Id_Fk",
                        column: x => x.User_Id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_favourites",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "integer", nullable: false),
                    Anime_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.User_Id, x.Anime_Id });
                    table.ForeignKey(
                        name: "User_Favourites_Anime_Id_fk",
                        column: x => x.Anime_Id,
                        principalTable: "anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "User_Favourites_User_Id_fk",
                        column: x => x.User_Id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Anime_English_Name_index",
                table: "anime",
                column: "English_Name");

            migrationBuilder.CreateIndex(
                name: "Anime_Episodes_index",
                table: "anime",
                column: "Episodes");

            migrationBuilder.CreateIndex(
                name: "Anime_Name_index",
                table: "anime",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "Anime_Release_Year_index",
                table: "anime",
                column: "Release_Year");

            migrationBuilder.CreateIndex(
                name: "Anime_Score_index",
                table: "anime",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "Anime_Source_Id_fk",
                table: "anime",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "Anime_Type_Id_fk",
                table: "anime",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "AnimeId",
                table: "anime_genre",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "GenreId",
                table: "anime_genre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "AnimeId1",
                table: "anime_licensor",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "LicensorId",
                table: "anime_licensor",
                column: "LicensorId");

            migrationBuilder.CreateIndex(
                name: "AnimeId2",
                table: "anime_producer",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "ProducerId",
                table: "anime_producer",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "Name",
                table: "genre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Name1",
                table: "licensor",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Name2",
                table: "producer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_User_Id",
                table: "refresh_token",
                column: "User_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Refresh_Token_Hashed_Token__index",
                table: "refresh_token",
                column: "Hashed_Token");

            migrationBuilder.CreateIndex(
                name: "IX_review_Anime_Id",
                table: "review",
                column: "Anime_Id");

            migrationBuilder.CreateIndex(
                name: "IX_review_User_Id",
                table: "review",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "Name3",
                table: "source",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Name4",
                table: "type",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Email",
                table: "user",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_Role_Id",
                table: "user",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_user_favourites_Anime_Id",
                table: "user_favourites",
                column: "Anime_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anime_genre");

            migrationBuilder.DropTable(
                name: "anime_licensor");

            migrationBuilder.DropTable(
                name: "anime_producer");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "user_favourites");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "licensor");

            migrationBuilder.DropTable(
                name: "producer");

            migrationBuilder.DropTable(
                name: "anime");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "source");

            migrationBuilder.DropTable(
                name: "type");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
