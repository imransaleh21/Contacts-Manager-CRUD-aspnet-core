using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Entities
{
    public class PersonsDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public PersonsDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        /// <summary>
        /// OnModelCreating is overridden from DbContext class and is used to configure the model
        /// like setting table names, relationships, primary keys, relationships, constraints, seeding data etc.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call the base method to ensure any base configurations are applied
            // Configure the table names for the entities
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons", t =>
            {
                // Fluent Api to add check constraint on PIN column of Persons table
                t.HasCheckConstraint("CHK_PIN", "len([PIN]) = 4");
            });

            // Seed initial data for Countries

            // we can seed data one by one like this
            //modelBuilder.Entity<Country>().HasData(
            //    new Country { CountryID = Guid.NewGuid(), CountryName = "United States" },
            //);

            // Or, also we can use a collection(like json array) to seed multiple records at once
            // like below uncommented code
            string countriesJson = System.IO.File.ReadAllText("countries.json");
           List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach(Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            // Seed initial data for Persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach(Person person in persons)
            {
                //person.PersonId = Guid.NewGuid(); // Ensure each person has a unique ID
                modelBuilder.Entity<Person>().HasData(person);
            }

            // Fluent Api example
            // Use Fluent Api to Configure the PIN property of Person entity to have a specific column type
            modelBuilder.Entity<Person>().Property(column => column.PIN)
                .HasColumnType("varchar(6)");

            //modelBuilder.Entity<Person>().HasIndex(c => c.PIN).IsUnique(); // to set unique column
        }

        /// <summary>
        /// Stored procedure to get all persons
        /// </summary>
        /// <returns></returns>
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public async Task<int> sp_InsertPerson(Person person)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@PersonId", person.PersonId),
                new SqlParameter("@PersonName", person.PersonName ?? (object)DBNull.Value),
                new SqlParameter("@Email", person.Email ?? (object)DBNull.Value),
                new SqlParameter("@DateOfBirth", person.DateOfBirth ?? (object)DBNull.Value),
                new SqlParameter("@Gender ", person.Gender ?? (object)DBNull.Value),
                new SqlParameter("@CountryId", person.CountryId ?? (object)DBNull.Value),
                new SqlParameter("@Address", person.Address ?? (object)DBNull.Value),
                new SqlParameter("@ReceiveNewsLettter", person.ReceiveNewsLettter ?? (object)DBNull.Value)
            };

            return await Database.ExecuteSqlRawAsync("EXECUTE [dbo].[InsertPerson] " +
                "@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId," +
                " @Address, @ReceiveNewsLettter", sqlParameters);
        }
    }
}
