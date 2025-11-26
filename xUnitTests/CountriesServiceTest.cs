using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;

namespace xUnitTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countryService;
        private readonly IFixture _fixture;
        public CountriesServiceTest()
        {
            // This shows the 
            var countriesInitialData = new List<Country>() { };
            DbContextMock<PersonsDbContext> dbContextMock = new DbContextMock<PersonsDbContext>(
                new DbContextOptionsBuilder<PersonsDbContext>().Options);
            PersonsDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(dbSet => dbSet.Countries, countriesInitialData);
            _countryService = new CountriesService(dbContext);
            _fixture = new Fixture();
        }

        #region AddCountry Tests
        // When CountryAddRequest is null, it should throw an ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountryRequest()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync< ArgumentNullException>(async ()=>
            {
                //Act
                await _countryService.AddCountry(request);
            });

        }
        //When CountryName is null or empty, it should throw an ArgumentException
        [Fact]
        public async Task AddCountry_NullCountryName()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                                                    .With(country => country.CountryName, null as string)
                                                    .Create();
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                //Act
                await _countryService.AddCountry(request);
            });

        }

        //When CountryName is Duplicate, it should throw an ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = _fixture.Build<CountryAddRequest>()
                .With(t => t.CountryName, "Bangladesh")
                .Create();
            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
                .With(t => t.CountryName, "Bangladesh")
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countryService.AddCountry(request1);
                await _countryService.AddCountry(request2);

            });

        }

        /*when CountryName is valid, it should insert the country in the list of countries
        and generate a proper CountryId*/
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Create<CountryAddRequest>();
            //Act
            CountryResponse countryResponse = await _countryService.AddCountry(request);
            List<CountryResponse> allCountries = await _countryService.GetAllCountries();

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Contains(countryResponse, allCountries);
        }
        #endregion
        #region GetAllCountries Tests
        //When Country list is empty, It will return a empty list
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //act
            List<CountryResponse> actual_country_response_list = await _countryService.GetAllCountries();

            //assert
            Assert.Empty(actual_country_response_list);
        }

        // When Country list is not empty, It will return a list of countries
        [Fact]
        public async Task GetAllCountryDetails_ListOfTheCountries()
        {
            //arrange
            List<CountryResponse> country_list_while_adding_countries = new();
            List<CountryResponse> country_list_while_calling_getAllCountries = new();
            List<CountryAddRequest> countryAddRequests = new() 
            {
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>()
            };

            //act
            foreach (var countryRequest in countryAddRequests) 
            {
                country_list_while_adding_countries.Add( await _countryService.AddCountry(countryRequest));
            }
            country_list_while_calling_getAllCountries = await _countryService.GetAllCountries();

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
        public async Task GetCountryByCountryId_NullCountryId()
        {
            // Arrange
            Guid? countryId = null;

            //Assert with Act
            Assert.Null(await _countryService.GetCountryByCountryId(countryId));
        }

        // When Appropriate CountryId is Found, it should return the appropriate Country
        [Fact]
        public async Task GetCountryByCountryId_AppropriateCountryById()
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

            //Act
            CountryResponse? countryById = await _countryService.GetCountryByCountryId(countryResponse.CountryID);

            //Assert
            Assert.Equal(countryById, countryResponse);

        }
        #endregion
    }
}