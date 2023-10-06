using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fbognini.i18n.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslationsForLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE [i18n].[AddTranslationsForLanguage] 
	-- Add the parameters for the stored procedure here
	@LanguageId NCHAR(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [i18n].[Translations] (LanguageId, TextId, ResourceId, Destination, Updated)
	SELECT @LanguageId, [Texts].[TextId], [Texts].[ResourceId], [Texts].[TextId], GETDATE()
	FROM [i18n].[Texts]
END
GO

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [i18n].[AddTranslationsForLanguage]");
        }
    }
}
