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

        #region AddCountry Tests
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
            List<CountryResponse> allCountries = _countryService.GetAllCountries();

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Contains(countryResponse, allCountries);
        }
        #endregion
        #region GetAllCountries Tests
        //When Country list is empty, It will return a empty list
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //act
            List<CountryResponse> actual_country_response_list = _countryService.GetAllCountries();

            //assert
            Assert.Empty(actual_country_response_list);
        }

        // When Country list is not empty, It will return a list of countries
        [Fact]
        public void GetAllCountryDetails_ListOfTheCountries()
        {
            //arrange
            List<CountryResponse> country_list_while_adding_countries = new();
            List<CountryResponse> country_list_while_calling_getAllCountries = new();
            List<CountryAddRequest> countryAddRequests = new() 
            {
                new() {CountryName = "Bangladesh"},
                new() {CountryName = "Pakistan"},
                new() {CountryName = "Afganistan"}
            };

            //act
            foreach (var countryRequest in countryAddRequests) 
            {
                country_list_while_adding_countries.Add(_countryService.AddCountry(countryRequest));
            }
            country_list_while_calling_getAllCountries = _countryService.GetAllCountries();

            //assert
            //Check if the country list while adding countries is same as the country list while calling GetAllCountries
            foreach (var expected_country in country_list_while_adding_countries)
            {
                Assert.Contains(expected_country, country_list_while_calling_getAllCountries);
            }
        }
        #endregion
        #region GetCountryByCountryId Tests
        //When CountryId is not found, it should return null
        [Fact]
        public void GetCountryByCountryId_NullCountryId()
        {
            // Arrange
            Guid? countryId = null;

            //Assert with Act
            Assert.Null(_countryService.GetCountryByCountryId(countryId));
        }

        // When Appropriate CountryId is Found, it should return the appropriate Country
        [Fact]
        public void GetCountryByCountryId_AppropriateCountryById()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new()
            {
                CountryName = "Bangladesh"
            };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            //Act
            CountryResponse? countryById = _countryService.GetCountryByCountryId(countryResponse.CountryID);

            //Assert
            Assert.Equal(countryById, countryResponse);

        }
        #endregion
    }
}