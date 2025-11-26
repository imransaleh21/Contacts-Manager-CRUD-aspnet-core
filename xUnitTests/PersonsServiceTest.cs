using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;

namespace xUnitTests
{
    public class PersonsServiceTest
    {

        private readonly ICountriesService _countriesService;
        private readonly IPersonsService _personsService;
        private readonly IFixture _fixture;

        public PersonsServiceTest()
        {
            _fixture = new Fixture();
            // Mock the DbContext and DbSets for Persons and Countries
            List<Country> countriesInitialData = new List<Country>() { };
            List<Person> personsInitialData = new List<Person>() { };
            // Creating DbContextMock
            DbContextMock<PersonsDbContext> dbContextMock = new DbContextMock<PersonsDbContext>(
                new DbContextOptionsBuilder<PersonsDbContext>().Options);
            PersonsDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(dbSet => dbSet.Persons, personsInitialData);
            dbContextMock.CreateDbSetMock(dbSet => dbSet.Countries, countriesInitialData);

            _countriesService = new CountriesService(dbContext);
            _personsService = new PersonsService(dbContext, _countriesService);
        }

        #region CreatePersonList
        /// <summary>
        /// This method creates a list of persons with some predefined data
        /// before calling the AddPerson it adds some countries to the countries service to get the country id
        /// </summary>
        /// <returns> List of PersonResponse</returns>
        internal async Task<List<PersonResponse>> CreatePersonList()
        {
            // At first add some countries to the countries service to get the country id
            CountryResponse countryResponse1 = await _countriesService.AddCountry(_fixture.Create<CountryAddRequest>());
            CountryResponse countryResponse2 = await _countriesService.AddCountry(_fixture.Create<CountryAddRequest>());
            CountryResponse countryResponse3 = await _countriesService.AddCountry(_fixture.Create<CountryAddRequest>());

            List<PersonResponse> personResponsesWhileAdding = new();

            // Add some persons to the persons service with the countries added above
            List<PersonAddRequest> personAddRequests = new()
            {
                _fixture.Build<PersonAddRequest>()
                .With(t => t.Email, "abc@gmail.com")
                .With(t => t.DateOfBirth, DateTime.Parse("1995-05-21"))
                .With(t => t.CountryId, countryResponse1.CountryID)
                .Create(),

                _fixture.Build<PersonAddRequest>()
                .With(t => t.PersonName, "Imran")
                .With(t => t.Email, "saleh@gmail.com")
                .With(t => t.DateOfBirth, DateTime.Parse("1990-10-24"))
                .With(t => t.CountryId, countryResponse2.CountryID)
                .Create(),

                _fixture.Build<PersonAddRequest>()
                .With(t => t.Email, "muhammad@gmail.com")
                .With(t => t.DateOfBirth, DateTime.Parse("1996-12-20"))
                .With(t => t.CountryId, countryResponse3.CountryID)
                .Create()
            };

            // Replace the foreach loop with a LINQ expression,
            // LINQ is used here as we need to convert each PersonAddRequest to PersonResponse
            foreach (PersonAddRequest personRequest in personAddRequests)
            {
                PersonResponse personResponse = await _personsService.AddPerson(personRequest);
                personResponsesWhileAdding.Add(personResponse);
            }

            return personResponsesWhileAdding;
        }
        #endregion

        #region AddPerson Tests
        [Fact]
        public async Task AddPerson_NullRequest()
        {
            //Arrange
            PersonAddRequest request = null;

            //Assert with Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _personsService.AddPerson(request));
        }

        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                PersonName = null
            };

            //Assert with Act
            await Assert.ThrowsAsync<ArgumentException>(async () => await _personsService.AddPerson(request));
        }

        [Fact]
        public async Task AddPerson_NullPersonEmail()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                Email = null
            };

            //Assert with Act
            await Assert.ThrowsAsync<ArgumentException>(async () => await _personsService.AddPerson(request));
        }

        [Fact]
        public async Task AddPerson_NullPersonReceiveNewsLettter()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                ReceiveNewsLettter = null
            };

            //Assert with Act
            await Assert.ThrowsAsync<ArgumentException>(async () => await _personsService.AddPerson(request));
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "saleh@student.ac.bd")
                .With(temp => temp.DateOfBirth, DateTime.Parse("2000-02-21"))
                .Create();

            //Act
            PersonResponse personResponse = await _personsService.AddPerson(request);
            List<PersonResponse> personResponseList = await _personsService.GetAllPersons();

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, personResponseList);
        }
        #endregion

        #region GetPersonByPersonId Tests
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? PersonId = null;

            //Assert with Act
            Assert.Null(await _personsService.GetPersonByPersonId(PersonId));
        }

        [Fact]
        public async Task GetPersonByPersonId_AppropriatePersonById()
        {
            //Arrange
            CountryAddRequest request = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse =  await _countriesService.AddCountry(request);
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "abc@gmail,bd")
                .With(temp => temp.DateOfBirth, DateTime.Parse("1990-05-21"))
                .With(temp => temp.CountryId, countryResponse.CountryID)
                .Create();

            PersonResponse personAddResponse = await _personsService.AddPerson(personAddRequest);
            Guid personId = personAddResponse.PersonId;

            //Act
            PersonResponse? getPersonResponseById = await _personsService.GetPersonByPersonId(personId);

            //Assert
            Assert.Equal(getPersonResponseById, personAddResponse);
        }
        #endregion

        #region GetAllPersons Tests
        [Fact]
        public async Task GetAllPersons_EmptyPersonList()
        {
            //Assert with Act
            Assert.Empty(await _personsService.GetAllPersons());
        }

        [Fact]
        public async Task GetAllPersons_ListOfPersons()
        {
            //Arrange
            List<PersonResponse> personResponsesWhileAdding = await CreatePersonList();

            //Act
            List<PersonResponse> personResponsesWhileGettingAllPerson = await _personsService.GetAllPersons();

            //Assert
            foreach (PersonResponse personResponse in personResponsesWhileAdding)
            {
                Assert.Contains(personResponse, personResponsesWhileGettingAllPerson);
            }
        }
        #endregion

        #region GetFilteredPersons Tests
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            List<PersonResponse> personResponsesWhileAdding = await CreatePersonList();

            //Act
            List<PersonResponse> filteredPersonResponse = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Assert
            foreach (PersonResponse personResponse in personResponsesWhileAdding)
            {
                Assert.Contains(personResponse, filteredPersonResponse);
            }
        }

        [Fact]
        public async Task GetFilteredPersons_ProperSearchTextForPersonName()
        {
            //Arrange
            List<PersonResponse> personResponsesWhileAdding = await CreatePersonList();

            //Act
            List<PersonResponse> filteredPersonResponse = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ra");

            //Assert
            foreach (PersonResponse personResponse in personResponsesWhileAdding)
            {
                if (personResponse.PersonName != null &&
                    personResponse.PersonName.Contains("ra", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponse, filteredPersonResponse);
                }
            }
        }
        #endregion
    }
}
