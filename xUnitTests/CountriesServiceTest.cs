using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace xUnitTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countryService;
        public CountriesServiceTest()
        {
            _countryService = new CountriesService();
        }

        // When CountryAddRequest is null, it should throw an ArgumentNullException
        [Fact]
        public void AddCountry_NullCountryRequest()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws< ArgumentNullException>(()=>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }
        //When CountryName is null or empty, it should throw an ArgumentException
        [Fact]
        public void AddCountry_NullCountryName()
        {
            //Arrange
            CountryAddRequest? request = new()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }

        //When CountryName is Duplicate, it should throw an ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new()
            {
                CountryName = "Bangladesh"
            };
            CountryAddRequest? request2 = new()
            {
                CountryName = "Bangladesh"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request1);
                _countryService.AddCountry(request2);

            });

        }

        /*when CountryName is valid, it should insert the country in the list of countries
        and generate a proper CountryId*/
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new()
            {
                CountryName = "Pakistan"
            };
            //Act
            CountryResponse countryResponse = _countryService.AddCountry(request);

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
        }
    }
}