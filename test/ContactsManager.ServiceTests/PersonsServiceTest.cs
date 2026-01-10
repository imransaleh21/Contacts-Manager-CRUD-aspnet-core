using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Linq.Expressions;

namespace xUnitTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;

        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;

        private readonly IFixture _fixture;

        public PersonsServiceTest()
        {
            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;

            _personsGetterService = new PersonsGetterService(
                _personsRepository,
                NullLogger<PersonsGetterService>.Instance);

            _personsAdderService = new PersonsAdderService(
                _personsRepository,
                NullLogger<PersonsAdderService>.Instance);

            _personsUpdaterService = new PersonsUpdaterService(
                _personsRepository,
                NullLogger<PersonsUpdaterService>.Instance);

            _personsDeleterService = new PersonsDeleterService(
                _personsRepository,
                NullLogger<PersonsDeleterService>.Instance);

            _personsSorterService = new PersonsSorterService(
                _personsRepository,
                NullLogger<PersonsSorterService>.Instance);
        }

        #region CreatePersonList
        /// <summary>
        /// This method creates a list of persons with some predefined data
        /// before calling the AddPerson it adds some countries to the countries service to get the country id
        /// </summary>
        /// <returns> List of PersonResponse</returns>
        internal List<Person> CreatePersonList()
        {
            // Add some persons to the persons service with the countries added above
            List<Person> persons = new()
            {
                _fixture.Build<Person>()
                .With(t => t.Email, "abc@gmail.com")
                .With(t => t.DateOfBirth, DateTime.Parse("1995-05-21"))
                .Create(),

                _fixture.Build<Person>()
                .With(t => t.PersonName, "Imran")
                .With(t => t.Email, "saleh@gmail.com")
                .With(t => t.DateOfBirth, DateTime.Parse("1990-10-24"))
                .Create(),

                _fixture.Build<Person>()
                .With(t => t.Email, "muhammad@gmail.com")
                .With(t => t.DateOfBirth, DateTime.Parse("1996-12-20"))
                .Create()
            };
            return persons;
        }
        #endregion

        #region AddPerson Tests
        [Fact]
        public async Task AddPerson_NullRequest_ToBeArgumentNullException()
        {
            //Arrange
            PersonAddRequest? request = null;
            //Assert with Act
            Func<Task> action = async () => await _personsAdderService.AddPerson(request);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_NullPersonName_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                PersonName = null
            };

            //Assert with Act
            Func<Task> action = async () => await _personsAdderService.AddPerson(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_NullPersonEmail_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                Email = null
            };

            //Assert with Act
            Func<Task> action = async () => await _personsAdderService.AddPerson(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_NullPersonReceiveNewsLettter_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                ReceiveNewsLettter = null
            };

            //Assert with Act
            Func<Task> action = async () => await _personsAdderService.AddPerson(request);
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            //Arrange
            PersonAddRequest request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "saleh@student.ac.bd")
                .With(temp => temp.DateOfBirth, DateTime.Parse("2000-02-21"))
                .Create();
            Person person = request.ToPerson();
            PersonResponse personResponse = person.ToPersonResponse();

            _personsRepositoryMock.Setup(mockFun => mockFun.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            //Act
            PersonResponse personResponseWhileAdd = await _personsAdderService.AddPerson(request);
            personResponse.PersonId = personResponseWhileAdd.PersonId;

            ////Assert
            personResponseWhileAdd.PersonId.Should().NotBe(Guid.Empty);
            personResponseWhileAdd.Should().BeEquivalentTo(personResponse);
        }
        #endregion

        #region GetPersonByPersonId Tests
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId_ToBeNull()
        {
            //Arrange
            Guid? PersonId = null;

            //Assert with Act
            PersonResponse? result = await _personsGetterService.GetPersonByPersonId(PersonId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByPersonId_AppropriatePersonById_ToBeSuccessfull()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "abc@gmail.com")
                .With(temp => temp.DateOfBirth, DateTime.Parse("1990-05-21"))
                .Create();
            PersonResponse expected_personResponse = person.ToPersonResponse();
            _personsRepositoryMock.Setup(mockFun => mockFun.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            //Act
            PersonResponse? getPersonResponseById = await _personsGetterService.GetPersonByPersonId(person.PersonId);

            //Assert
            // fluent assertion gives error in comparing two PersonResponse objects
            getPersonResponseById.Should().BeEquivalentTo(expected_personResponse);
        }
        #endregion

        #region GetAllPersons Tests
        [Fact]
        public async Task GetAllPersons_ToBeEmptyPersonList()
        {
            //Arrange
            _personsRepositoryMock.Setup(mockFun => mockFun.GetAllPersons())
                .ReturnsAsync(new List<Person>());
            //Act
            var persons = await _personsGetterService.GetAllPersons();
            //Assert
            persons.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> personList = CreatePersonList();
            _personsRepositoryMock.Setup(mockFun => mockFun.GetAllPersons())
                .ReturnsAsync(personList);
            List<PersonResponse> personResponse = personList.Select(p => p.ToPersonResponse()).ToList();
            //Act
            List<PersonResponse> personResponsesWhileGettingAllPerson = await _personsGetterService.GetAllPersons();
            //Assert
            personResponsesWhileGettingAllPerson.Should().BeEquivalentTo(personResponse);
        }
        #endregion

        #region GetFilteredPersons Tests
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            List<Person> personList = CreatePersonList();
            _personsRepositoryMock.Setup(mockFun => mockFun.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(personList);
            List<PersonResponse> personResponsesOfPersonList = personList.Select(p => p.ToPersonResponse()).ToList();
            //Act
            List<PersonResponse> filteredPersonResponse = await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Assert
            filteredPersonResponse.Should().BeEquivalentTo(personResponsesOfPersonList);
        }

        [Fact]
        public async Task GetFilteredPersons_ProperSearchTextForPersonName()
        {
            //Arrange
            List<Person> personList = CreatePersonList();
            _personsRepositoryMock.Setup(mockFun => mockFun.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                 .ReturnsAsync(new List<Person> { personList[1] });
            //Act
            List<PersonResponse> filteredPersonResponse = await _personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "ra");

            //Assert
            filteredPersonResponse.Should().OnlyContain(pr =>
            pr.PersonName.Contains("ra", StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}
