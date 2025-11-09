using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Threading.Tasks;

namespace xUnitTests
{
    public class PersonsServiceTest
    {

        private readonly ICountriesService _countriesService;
        private readonly IPersonsService _personsService;

        public PersonsServiceTest()
        {
            /* As the PersonsService use DI Container to inject the CountriesService, but when we use xUnit tests,
            we cannot use the the program.cs file DI Container, so we have to create the CountriesService object manually and
            pass it to the PersonsService constructor. */
            /* In future, we can use a mocking framework like Moq to mock the CountriesService */

            _countriesService = new CountriesService(
                new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));

            _personsService = new PersonsService(
                new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
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
            CountryResponse countryResponse1 = await _countriesService.AddCountry(new CountryAddRequest() { CountryName = "Bangladesh" });
            CountryResponse countryResponse2 = await _countriesService.AddCountry(new CountryAddRequest() { CountryName = "Pakistan" });
            CountryResponse countryResponse3 = await _countriesService.AddCountry(new CountryAddRequest() { CountryName = "Afganisthan" });

            List<PersonResponse> personResponsesWhileAdding = new();

            // Add some persons to the persons service with the countries added above
            List<PersonAddRequest> personAddRequests = new()
            {
                new()
                {
                    PersonName = "Imran Muhammad",
                    Email = "imran@gmail.com",
                    DateOfBirth = DateTime.Parse("2000-02-21"),
                    Gender = GenderOptions.Male,
                    CountryId = countryResponse1.CountryID,
                    Address = "Mohammadpur, Dhaka",
                    ReceiveNewsLettter = true
                },

                new()
                {
                    PersonName = "Emran Saleh",
                    Email = "saleh@gmail.com",
                    DateOfBirth = DateTime.Parse("1999-02-02"),
                    Gender = GenderOptions.Male,
                    CountryId = countryResponse2.CountryID,
                    Address = "Birol, Dinajpur",
                    ReceiveNewsLettter = true
                },

                new()
                {
                    PersonName = "Muhammad Khan",
                    Email = "muhammad@gmail.com",
                    DateOfBirth = DateTime.Parse("1998-09-11"),
                    Gender = GenderOptions.Male,
                    CountryId = countryResponse3.CountryID,
                    Address = "Kabul",
                    ReceiveNewsLettter = false
                }
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
            PersonAddRequest request = new()
            {
                PersonName = "Imran Saleh",
                Email = "imran@gmail.com",
                DateOfBirth = DateTime.Parse("2000-02-21"),
                Gender = GenderOptions.Male,
                CountryId = Guid.NewGuid(),
                Address = "Mohammadpur, Dhaka",
                ReceiveNewsLettter = true
            };

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
            PersonAddRequest personAddRequest = new()
            {
                PersonName = "Muhammad",
                Email = "muhammad@gmail.com",
                DateOfBirth = DateTime.Parse("1996-02-21"),
                Gender = GenderOptions.Male,
                CountryId = Guid.NewGuid(),
                Address = "Birol, Dinajpur",
                ReceiveNewsLettter = false
            };
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
            List<PersonResponse> filteredPersonResponse = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ha");

            //Assert
            foreach (PersonResponse personResponse in personResponsesWhileAdding)
            {
                if (personResponse.PersonName != null &&
                    personResponse.PersonName.Contains("ha", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponse, filteredPersonResponse);
                }
            }
        }
        #endregion
    }
}
