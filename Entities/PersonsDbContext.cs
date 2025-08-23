using Microsoft.EntityFrameworkCore;
namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

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
            modelBuilder.Entity<Person>().ToTable("Persons");

            // Seed initial data for Countries

            // we can seed data one by one like this or
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
                modelBuilder.Entity<Person>().HasData(person);
            }

        }
    }
}
