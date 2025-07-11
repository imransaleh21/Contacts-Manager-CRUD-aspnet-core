using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        private readonly List<CountryResponse> _countriesResponse;

        public CountriesService()
        {
            _countries = new List<Country>();
            _countriesResponse = new List<CountryResponse>();
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
    }
}
