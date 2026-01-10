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
        private readonly IPersonsGetterService _personsGettterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;
        private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;
        private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;
        private readonly Mock<ICountriesService> _countriesServiceMock;

        private readonly Mock<ILogger<PersonsController>> _loggerMock;
        private readonly Fixture _fixture;
        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _personsGetterServiceMock = new Mock<IPersonsGetterService>();
            _personsGettterService = _personsGetterServiceMock.Object;
            _personsAdderServiceMock = new Mock<IPersonsAdderService>();
            _personsAdderService = _personsAdderServiceMock.Object;
            _personsSorterServiceMock = new Mock<IPersonsSorterService>();
            _personsSorterService = _personsSorterServiceMock.Object;
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
            PersonsController personsController = new(
                _personsGettterService,
                _personsAdderService,
                _personsSorterService,
                _countriesService,
                _loggerMock.Object
                );

            _personsGetterServiceMock.Setup(_personsServiceMock =>
            _personsServiceMock.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponses);

            _personsSorterServiceMock.Setup(_personsServiceMock =>
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
        // As invalid model state code is now shifted to filter, this test is no longer valid.
        //[Fact]
        //public async Task Create_PostRequest_IfModelStateIsInValid_ReturnsCreateView()
        //{
        //    //Arrange
        //    PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();
        //    PersonResponse personResponse = _fixture.Create<PersonResponse>();
        //    List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

        //    _personsServiceMock.Setup(_personsServiceMock => _personsServiceMock.AddPerson(It.IsAny<PersonAddRequest>()))
        //        .ReturnsAsync(personResponse);
        //    _countriesServiceMock.Setup(_countriesServiceMock => _countriesServiceMock.GetAllCountries())
        //        .ReturnsAsync(countryResponses);

        //    PersonsController personsController = new(_personsService, _countriesService, _loggerMock.Object);

        //    //Act
        //    personsController.ModelState.AddModelError("PersonName", "Person Name is required");
        //    IActionResult result = await personsController.Create(personAddRequest);
        //    //Assert
        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);
        //    viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        //    viewResult.ViewData.Model.Should().BeEquivalentTo(personAddRequest);
        //}

        [Fact]
        public async Task Create_PostRequest_IfModelStateIsValid_RedirectToIndexView()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();
            List<CountryResponse> countryResponses = _fixture.Create<List<CountryResponse>>();

            _personsAdderServiceMock.Setup(_personsServiceMock => _personsServiceMock.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);
            _countriesServiceMock.Setup(_countriesServiceMock => _countriesServiceMock.GetAllCountries())
                .ReturnsAsync(countryResponses);

            PersonsController personsController = new(
                        _personsGettterService,
                        _personsAdderService,
                        _personsSorterService,
                        _countriesService,
                        _loggerMock.Object
                    );

            //Act
            IActionResult result = await personsController.Create(personAddRequest);
            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}
