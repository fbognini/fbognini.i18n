using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fbognini.i18n.Persistence.Migrations
{
    public partial class I18N_InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "i18n");

            migrationBuilder.CreateTable(
                name: "Configurations",
                schema: "i18n",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nchar(5)", fixedLength: true, maxLength: 5, nullable: false),
                    BaseUriResource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "i18n",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nchar(5)", fixedLength: true, maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Texts",
                schema: "i18n",
                columns: table => new
                {
                    TextId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResourceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => new { x.TextId, x.ResourceId });
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                schema: "i18n",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "nchar(5)", nullable: false),
                    TextId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ResourceId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => new { x.LanguageId, x.TextId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_Translations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "i18n",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translations_Texts_TextId_ResourceId",
                        columns: x => new { x.TextId, x.ResourceId },
                        principalSchema: "i18n",
                        principalTable: "Texts",
                        principalColumns: new[] { "TextId", "ResourceId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translations_TextId_ResourceId",
                schema: "i18n",
                table: "Translations",
                columns: new[] { "TextId", "ResourceId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations",
                schema: "i18n");

            migrationBuilder.DropTable(
                name: "Translations",
                schema: "i18n");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "i18n");

            migrationBuilder.DropTable(
                name: "Texts",
                schema: "i18n");
        }
    }
}
