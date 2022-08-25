using Microsoft.EntityFrameworkCore.Migrations;

namespace fbognini.i18n.Persistence.Migrations
{
    public partial class FixForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LanguageId",
                schema: "i18n",
                table: "Translations",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LanguageId",
                schema: "i18n",
                table: "Translations",
                type: "nvarchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 5);
        }
    }
}
