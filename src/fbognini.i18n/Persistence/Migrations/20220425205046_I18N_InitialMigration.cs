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
                    BaseUriResource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                schema: "i18n",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => new { x.LanguageId, x.Source });
                });

            migrationBuilder.Sql(@"
                INSERT INTO [i18n].[Configurations] (
		            [Id]
		            ,[BaseUriResource]
		            ,[Sequence]
	            )
	            VALUES (
		            'TRA'
		            ,''
		            ,0
	            )
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [i18n].[GetNextSequence] 
					@Id NCHAR(3) = 'TRA'
				AS
				BEGIN
					-- SET NOCOUNT ON added to prevent extra result sets from
					-- interfering with SELECT statements.
					SET NOCOUNT ON;
	
					DECLARE @Sequence AS INT

					BEGIN TRAN
						SELECT @Sequence = Sequence FROM i18n.Configurations WHERE Id = @Id;
						UPDATE i18n.Configurations SET Sequence = Sequence + 1  WHERE Id = @Id;
					COMMIT TRAN

					RETURN @Sequence
				END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [i18n].[NewTranslation] 
					@Id INT = NULL,
					@DefaultString NVARCHAR(512) = NULL
				AS
				BEGIN
					-- SET NOCOUNT ON added to prevent extra result sets from
					-- interfering with SELECT statements.
					SET NOCOUNT ON;
 

					IF @Id IS NULL
						EXEC @Id = i18n.GetNextSequence
	
					IF @DefaultString IS NULL
						SET @DefaultString = ''

					DECLARE @user AS VARCHAR(100) = '[i18n].[NewTranslation]'
					DECLARE @now as datetime = GETDATE()

					INSERT INTO [i18n].[Translations]
						   ([LanguageId]
						   ,[Source]
						   ,[Destination]
						   )
					SELECT Id
							, @Id
							, @DefaultString
					FROM [i18n].[Languages]

					SELECT @Id AS [NewId];

					RETURN @Id
     
				END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations",
                schema: "i18n");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "i18n");

            migrationBuilder.DropTable(
                name: "Translations",
                schema: "i18n");

            migrationBuilder.Sql(@"
                DROP PROCEDURE [i18n].[GetNextSequence]
            ");

            migrationBuilder.Sql(@"
                DROP PROCEDURE [i18n].[NewTranslation] 
            ");
        }
    }
}
