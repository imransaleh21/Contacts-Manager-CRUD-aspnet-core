using AutoFixture;
using Contacts_Manager_CRUD.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Threading.Tasks;

namespace xUnitTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly Mock<ICountriesService> _countriesServiceMock;

        private readonly Mock<ILogger<PersonsController>> _loggerMock;
        private readonly Fixture _fixture;
        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _personsServiceMock = new Mock<IPersonsService>();
            _personsService = _personsServiceMock.Object;
            _countriesServiceMock = new Mock<ICountriesService>();
            _countriesService = _countriesServiceMock.Object;
            _loggerMock = new Mock<ILogger<PersonsController>>();
        }

        #region Index Controller Tests
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPersons()
        {
            //Arrange
            List<PersonResponse> personResponses = _fixture.Create<List<PersonResponse>>();
            PersonsController personsController = new(_personsService, _countriesService, _loggerMock.Object);

            _personsServiceMock.Setup(_personsServiceMock => 
            _personsServiceMock.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponses);

            _personsServiceMock.Setup(_personsServiceMock =>
            _personsServiceMock.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .Returns(personResponses);
            //Act
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(),
                _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<PersonResponse>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponses);
        }
        #endregion
        #region Create Person Controller Tests
        [Fact]
        public async Task Create_PostRequest_IfModelStateIsInValid_ReturnsCreateView()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();
            List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

            _personsServiceMock.Setup(_personsServiceMock => _personsServiceMock.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);
            _countriesServiceMock.Setup(_countriesServiceMock => _countriesServiceMock.GetAllCountries())
                .ReturnsAsync(countryResponses);

            PersonsController personsController = new(_personsService, _countriesService, _loggerMock.Object);

            //Act
            personsController.ModelState.AddModelError("PersonName", "Person Name is required");
            IActionResult result = await personsController.Create(personAddRequest);
            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personAddRequest);
        }

        [Fact]
        public async Task Create_PostRequest_IfModelStateIsValid_RedirectToIndexView()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();
            List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

            _personsServiceMock.Setup(_personsServiceMock => _personsServiceMock.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);
            _countriesServiceMock.Setup(_countriesServiceMock => _countriesServiceMock.GetAllCountries())
                .ReturnsAsync(countryResponses);

            PersonsController personsController = new(_personsService, _countriesService, _loggerMock.Object);

            //Act
            IActionResult result = await personsController.Create(personAddRequest);
            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
