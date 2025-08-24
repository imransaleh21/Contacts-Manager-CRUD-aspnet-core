using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext dbContext)
        {
            _db = dbContext;
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            // validation: countryAddRequest should not be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            // validation: countryAddRequest.CountryName should not be null
            else if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            // validation: Duplicate country name should not be allowed
            else if (_db.Countries
                .Where(country => country.CountryName == countryAddRequest.CountryName)
                .Count() > 0)
            {
                throw new ArgumentException($"Country with name {countryAddRequest.CountryName} already exists.");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            // Add country to Countries DbSet and save changes
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries
                .Select(country => country.ToCountryResponse())
                .ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                return null; // If countryId is null, return null
            }
            else
            {
                Country? country = _db.Countries
                    .FirstOrDefault(country => country.CountryID == countryId);
                return country?.ToCountryResponse() ?? null;
            }  
        }
    }
}
