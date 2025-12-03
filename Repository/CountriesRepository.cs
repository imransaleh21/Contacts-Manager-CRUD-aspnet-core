using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly PersonsDbContext _db;
        public CountriesRepository(PersonsDbContext db)
        {
            _db = db;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryId(Guid? countryId)
        {
            return await _db.Countries.FirstOrDefaultAsync(c => c.CountryID == countryId);
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            return await _db.Countries.FirstOrDefaultAsync(country => country.CountryName == countryName);
        }
    }
}
