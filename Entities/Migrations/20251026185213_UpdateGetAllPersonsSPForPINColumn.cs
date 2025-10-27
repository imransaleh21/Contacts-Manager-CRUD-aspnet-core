using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetAllPersonsSPForPINColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                ALTER PROCEDURE [dbo].[GetAllPersons]
                AS BEGIN
                    SELECT PersonId, PersonName, Email, DateOfBirth,
                    Gender, CountryId, Address, ReceiveNewsLettter, PIN
                    FROM [dbo].[Persons]
                END
            ";
            migrationBuilder.Sql(sp_GetAllPersons);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                ALTER PROCEDURE [dbo].[GetAllPersons]
                AS BEGIN
                    SELECT PersonId, PersonName, Email, DateOfBirth,
                    Gender, CountryId, Address, ReceiveNewsLettter
                    FROM [dbo].[Persons]
                END
            ";
            migrationBuilder.Sql(sp_GetAllPersons);

        }
    }
}
