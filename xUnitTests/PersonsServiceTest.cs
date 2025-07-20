using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;

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
            pass it to the PersonsService constructor.*/
            /* In future, we can use a mocking framework like Moq to mock the CountriesService */

            _countriesService = new CountriesService();
            _personsService = new PersonsService(_countriesService);
        }

        #region AddPerson Tests
        // If the request is null, then it should throw an ArgumentNullException.
        [Fact]
        public void AddPerson_NullRequest()
        {
            //Arrange
            PersonAddRequest request = null;

            //Assert with Act
            Assert.Throws<ArgumentNullException>(() => _personsService.AddPerson(request));
        }

        //If the person name is null, then it should throw an ArgumentException.
        [Fact]
        public void AddPerson_NullPersonName() 
        {
            //Arrange
            PersonAddRequest request = new()
            {
                PersonName = null
            };

            //Assert with Act
            Assert.Throws<ArgumentException>(() => _personsService.AddPerson(request));
        }

        //If the person email is null, then it should throw an ArgumentException.
        [Fact]
        public void AddPerson_NullPersonEmail()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                Email = null
            };

            //Assert with Act
            Assert.Throws<ArgumentException>(() => _personsService.AddPerson(request));
        }

        //If the person ReceiveNewsLettter is null, then it should throw an ArgumentException.
        [Fact]
        public void AddPerson_NullPersonReceiveNewsLettter()
        {
            //Arrange
            PersonAddRequest request = new()
            {
                ReceiveNewsLettter = null
            };

            //Assert with Act
            Assert.Throws<ArgumentException>(() => _personsService.AddPerson(request));
        }

        //If proper person details is provided then it should insert into the person list. Also it should
        // return the PersonResponse Object with newly generated person Id
        [Fact]
        public void AddPerson_ProperPersonDetails() 
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
            PersonResponse personResponse = _personsService.AddPerson(request);
            List<PersonResponse> personResponseList = _personsService.GetAllPersons();

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, personResponseList);

        }
        #endregion
        #region GetPersonByPersonId Tests
        // when person id is null, it should return null
        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? PersonId = null;
            //Assert with Act
            Assert.Null(_personsService.GetPersonByPersonId(PersonId));

        }

        // Get Appropriate person if proper person id is provided
        [Fact]
        public void GetPersonByPersonId_AppropriatePersonById()
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
            PersonResponse personAddResponse = _personsService.AddPerson(personAddRequest); // Add a person with details
            Guid personId = personAddResponse.PersonId; // get the person's personId

            //Act
            PersonResponse getPersonResponseById = _personsService.GetPersonByPersonId(personId); //get the specific person by it's id

            //Assert
            Assert.Equal(getPersonResponseById, personAddResponse);

        }
        #endregion
        #region GetAllPersons Tests
        //If the person list is empty then retun empty list
        [Fact]
        public void GetAllPersons_EmptyPersonList()
        {
            //Assert with Act
            Assert.Empty(_personsService.GetAllPersons());

        }

        //Get all the person's details in the list
        [Fact]
        public void GetAllPersons_ListOfPersons()
        {
            //Arrange
            List<PersonResponse> personResponsesWhileAdding = new();
            List<PersonResponse> personResponsesWhileGettingAllPerson = new();
            List<PersonAddRequest> personAddRequests = new()
            {
                new()
                {
                    PersonName = "Imran",
                    Email = "imran@gmail.com",
                    DateOfBirth = DateTime.Parse("2000-02-21"),
                    Gender = GenderOptions.Male,
                    CountryId = Guid.NewGuid(),
                    Address = "Mohammadpur, Dhaka",
                    ReceiveNewsLettter = true
                },

                new()
                {
                    PersonName = "Saleh",
                    Email = "saleh@gmail.com",
                    DateOfBirth = DateTime.Parse("1999-02-21"),
                    Gender = GenderOptions.Male,
                    CountryId = Guid.NewGuid(),
                    Address = "Birol, Dinajpur",
                    ReceiveNewsLettter = true
                }

            };

            // Replace the foreach loop with a LINQ expression,
            // LINQ is used here as we need to convert each PersonAddRequest to PersonResponse
            personResponsesWhileAdding = personAddRequests
                .Select(person => _personsService.AddPerson(person))
                .ToList();

            //Act
            personResponsesWhileGettingAllPerson = _personsService.GetAllPersons();

            //Assert
            // here LINQ expression is not used,
            // as we need to check if the personResponsesWhileAdding contains all the persons in personResponsesWhileGettingAllPerson
            foreach (PersonResponse personResponse in personResponsesWhileAdding)
            {
                Assert.Contains(personResponse, personResponsesWhileGettingAllPerson);
            }

        }

        #endregion
    }
}
