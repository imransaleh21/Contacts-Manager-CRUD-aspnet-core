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
using FluentAssertions;
using RepositoryContracts;
using Repository;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace xUnitTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly IPersonsRepository _personsRepository;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly Mock<ILogger<PersonsService>> _loggerMock;

        private readonly IFixture _fixture;

        public PersonsServiceTest()
        {
            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _loggerMock = new Mock<ILogger<PersonsService>>();
            _personsRepository = _personsRepositoryMock.Object;
            _personsService = new PersonsService(_personsRepository, _loggerMock.Object);
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
            Func<Task> action = async () => await _personsService.AddPerson(request);
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
            Func<Task> action = async () => await _personsService.AddPerson(request);
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
            Func<Task> action = async () => await _personsService.AddPerson(request);
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
            Func<Task> action = async () => await _personsService.AddPerson(request);
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
            PersonResponse personResponseWhileAdd = await _personsService.AddPerson(request);
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
            PersonResponse? result = await _personsService.GetPersonByPersonId(PersonId);
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
            PersonResponse? getPersonResponseById = await _personsService.GetPersonByPersonId(person.PersonId);

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
            var persons = await _personsService.GetAllPersons();
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
            List<PersonResponse> personResponsesWhileGettingAllPerson = await _personsService.GetAllPersons();
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
            List<PersonResponse> filteredPersonResponse = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

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
            List<PersonResponse> filteredPersonResponse = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ra");

            //Assert
            filteredPersonResponse.Should().OnlyContain(pr =>
            pr.PersonName.Contains("ra", StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}
