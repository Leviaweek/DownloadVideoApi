using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VideoDownloaderApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:public.media_type", "muxed_video,audio");

            migrationBuilder.CreateTable(
                name: "YoutubeVideos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InternalVideoId = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalYoutubeMedia",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YoutubeVideoId = table.Column<int>(type: "integer", nullable: false),
                    Quality = table.Column<int>(type: "integer", nullable: true),
                    Bitrate = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Format = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDownloaded = table.Column<bool>(type: "boolean", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalYoutubeMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalYoutubeMedia_YoutubeVideos_YoutubeVideoId",
                        column: x => x.YoutubeVideoId,
                        principalSchema: "public",
                        principalTable: "YoutubeVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeVideoLinks",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YoutubeVideoId = table.Column<int>(type: "integer", nullable: false),
                    VideoUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeVideoLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeVideoLinks_YoutubeVideos_YoutubeVideoId",
                        column: x => x.YoutubeVideoId,
                        principalSchema: "public",
                        principalTable: "YoutubeVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalYoutubeMedia_YoutubeVideoId",
                schema: "public",
                table: "PhysicalYoutubeMedia",
                column: "YoutubeVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideoLinks_YoutubeVideoId",
                schema: "public",
                table: "YoutubeVideoLinks",
                column: "YoutubeVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_InternalVideoId",
                schema: "public",
                table: "YoutubeVideos",
                column: "InternalVideoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhysicalYoutubeMedia",
                schema: "public");

            migrationBuilder.DropTable(
                name: "YoutubeVideoLinks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "YoutubeVideos",
                schema: "public");
        }
    }
}
