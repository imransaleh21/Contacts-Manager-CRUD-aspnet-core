using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp_InsertPerson = @"
            CREATE PROCEDURE [dbo].[InsertPerson]
            (@PersonId uniqueidentifier, @PersonName nvarchar(45),
            @Email nvarchar(30), @DateOfBirth datetime2(7),
            @Gender nvarchar(10), @CountryId uniqueidentifier, @Address nvarchar(65),
            @ReceiveNewsLettter bit)
            AS BEGIN
            INSERT INTO [dbo].[Persons]
            (PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLettter)
            VALUES
            (@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLettter)
            END
           ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp_InsertPerson = @"
            DROP PROCEDURE [dbo].[InsertPerson]
           ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
