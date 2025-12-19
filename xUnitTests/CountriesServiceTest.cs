using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Repository;

namespace xUnitTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countryService;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

        private readonly IFixture _fixture;
        public CountriesServiceTest()
        {
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;
            _countryService = new CountriesService(_countriesRepository);

            _fixture = new Fixture();
        }

        #region AddCountry Tests
        // When CountryAddRequest is null, it should throw an ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountryRequest_ToBeArgumentNullException()
        {
            //Arrange
            CountryAddRequest? request = null;
            //Assert with Act
            Func<Task> action = async () => await _countryService.AddCountry(request);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        //When CountryName is null or empty, it should throw an ArgumentException
        [Fact]
        public async Task AddCountry_NullCountryName_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                                                    .With(country => country.CountryName, null as string)
                                                    .Create();
            //Assert
            Func<Task> action = async () => await _countryService.AddCountry(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When CountryName is Duplicate, it should throw an ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest? request1 = _fixture.Build<CountryAddRequest>()
                .With(t => t.CountryName, "Bangladesh")
                .Create();
            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
                .With(t => t.CountryName, "Bangladesh")
                .Create();

            Country? country1 = request1.ToCountry();
            Country? country2 = request2.ToCountry();

            _countriesRepositoryMock.Setup(mockFun => mockFun.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(null as Country);
            _countriesRepositoryMock.Setup(mockFun => mockFun.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country1);
            await _countryService.AddCountry(request1);
            //Act
            Func<Task> action = async () =>
            {
                _countriesRepositoryMock.Setup(mockFun => mockFun.GetCountryByCountryName(It.IsAny<string>()))
                    .ReturnsAsync(country1);
                _countriesRepositoryMock.Setup(mockFun => mockFun.AddCountry(It.IsAny<Country>()))
                    .ReturnsAsync(country1);
                await _countryService.AddCountry(request2);

            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /*when CountryName is valid, it should insert the country in the list of countries
        and generate a proper CountryId*/
        [Fact]
        public async Task AddCountry_FullCountry_ToBeSuccessful()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Create<CountryAddRequest>();
            Country? country = request.ToCountry();
            CountryResponse countryResponse = country.ToCountryResponse();

            _countriesRepositoryMock.Setup(mockFun => mockFun.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(null as Country);
            _countriesRepositoryMock.Setup(mockFun => mockFun.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);
            //Act
            CountryResponse countryResponseWhileAdd = await _countryService.AddCountry(request);
            countryResponse.CountryID = countryResponseWhileAdd.CountryID;
            //Assert
            countryResponseWhileAdd.Should().NotBe(Guid.Empty);
            countryResponseWhileAdd.Should().BeEquivalentTo(countryResponse);
        }
        #endregion
        #region GetAllCountries Tests
        //When Country list is empty, It will return a empty list
        [Fact]
        public async Task GetAllCountries_CountryListToBeEmpty()
        {
            //Arrange
            _countriesRepositoryMock.Setup(mockFun => mockFun.GetAllCountries())
                .ReturnsAsync(new List<Country>());
            //act
            List<CountryResponse> actual_country_response_list = await _countryService.GetAllCountries();

            //assert
            actual_country_response_list.Should().BeEmpty();
        }

        // When Country list is not empty, It will return a list of countries
        [Fact]
        public async Task GetAllCountries_ToBeListOfTheCountries()
        {
            //arrange
            List<CountryResponse> country_list_while_calling_getAllCountries = new();
            List<Country> countryList = new()
            {
                _fixture.Create<Country>(),
                _fixture.Create<Country>(),
                _fixture.Create<Country>()
            };
            List<CountryResponse> country_Responses_Of_The_CountryList = countryList
                .Select(country => country.ToCountryResponse())
                .ToList();
            _countriesRepositoryMock.Setup(mockFun => mockFun.GetAllCountries())
                .ReturnsAsync(countryList);
            //act
            country_list_while_calling_getAllCountries = await _countryService.GetAllCountries();
            //assert
            country_list_while_calling_getAllCountries.Should().BeEquivalentTo(country_Responses_Of_The_CountryList);
        }
        #endregion
        #region GetCountryByCountryId Tests
        //When CountryId is not found, it should return null
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId_ToBeNull()
        {
            // Arrange
            Guid? countryId = null;
            _countriesRepositoryMock.Setup(mockFun => mockFun.GetCountryByCountryId(It.IsAny<Guid?>()))
                .ReturnsAsync(null as Country);
            //Act
            var result = await _countryService.GetCountryByCountryId(countryId);
            //Assert
            result.Should().BeNull();
        }

        // When Appropriate CountryId is Found, it should return the appropriate Country
        [Fact]
        public async Task GetCountryByCountryId_ToBeAppropriateCountry()
        {
            //Arrange
            Country country = _fixture.Create<Country>();
            CountryResponse countryResponse = country.ToCountryResponse();

            _countriesRepositoryMock.Setup(mockFun => mockFun.GetCountryByCountryId(It.IsAny<Guid?>()))
                .ReturnsAsync(country);
            //Act
            CountryResponse? returnCountryResponse = await _countryService.GetCountryByCountryId(countryResponse.CountryID);

            //Assert
            returnCountryResponse.Should().BeEquivalentTo(countryResponse);
        }
        #endregion
    }
}