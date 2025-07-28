using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool  mockData = true)
        {
            _countries = new List<Country>();
            if (mockData)
            {
                // Adding mock data for testing purposes. when we add EF Core later, we will remove this.
                _countries.Add(new Country
                {
                    CountryID = Guid.Parse("C011C5AE-9633-47ED-8B8D-1FDC330767C5"),
                    CountryName = "Bangladesh"
                });
                _countries.Add(new Country
                {
                    CountryID = Guid.Parse("F86235E7-A65D-45BD-8B49-9478FFF80C33"),
                    CountryName = "Pakistan"
                });
                _countries.Add(new Country
                {
                    CountryID = Guid.Parse("A0F1B2C3-D4E5-6789-ABCD-EF0123456789"),
                    CountryName = "Saudi Arabia"
                });
                _countries.Add(new Country
                {
                    CountryID = Guid.Parse("9AF86740-0EB0-4EE0-9087-E888C57DCD39"),
                    CountryName = "Iran"
                });
                _countries.Add(new Country
                {
                    CountryID = Guid.Parse("A171E94A-C261-4B8E-9148-BBEC92C7CC71"),
                    CountryName = "Iraq"
                });
            }
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
            else if (_countries
                .Where(country => country.CountryName == countryAddRequest.CountryName)
                .Count() > 0)
            {
                throw new ArgumentException($"Country with name {countryAddRequest.CountryName} already exists.");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                return null; // If countryId is null, return null
            }
            else
            {
                Country? country = _countries.FirstOrDefault(country => country.CountryID == countryId);
                return country?.ToCountryResponse() ?? null;
            }  
        }
    }
}
