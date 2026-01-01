using Contacts_Manager_CRUD.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Contacts_Manager_CRUD.Controllers
{
    [Route("[controller]")] // This is as same as [Route("persons")]
    //[Route("persons")]
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "Controller-Custom-key", "Custom-value", 2 }, Order = 2)]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonsService personsService,
            ICountriesService countriesService,
            ILogger<PersonsController> logger)
        {
            _personsService = personsService;
            _countriesService = countriesService;
            _logger = logger;
        }
        /// <summary>
        /// Index action method for the PersonsController. This will return the view of a list of persons based on different criteria
        /// </summary>
        /// <returns>Return the view with the list of persons</returns>
        [Route("[action]")] // This route is work same as the below one
        //[Route("index")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter), Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[]{"Index-Custom-key", "Custom-value", 1}, Order = 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        public async Task<IActionResult> Index(string searchBy, string searchValue,
            string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation($"Index Controller. Param: searchBy: {searchBy}, searchValue: {searchValue}, sortBy: {sortBy}, sortOrder: {sortOrder}");
            // Retrieve all persons from the service
            // as filtered person method returns all persons if no search criteria is provided
            // so this code is now commented
            //List<PersonResponse> allPersons = _personsService.GetAllPersons();
            //return View(allPersons);

            // based on the search criteria, filtered persons will be returned
            // and if no search criteria is provided, all persons will be returned
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchValue);

            // This viewbag code is now moved to PersonsListActionFilter filter's onActionExecuted method
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchValue = searchValue;

            // Code for sorting persons based on the sort criteria
            List<PersonResponse>? sortedPersons = _personsService.GetSortedPersons( persons, sortBy, sortOrder);

            // This viewbag code is now moved to PersonsListActionFilter filter's onActionExecuted method
            //ViewBag.CurrentSortBy = sortBy;
            //ViewBag.CurrentSortOrder = sortOrder;
            return View(sortedPersons); // Return the view with the list of persons at Views/Persons/Index.cshtml
        }

        #region Get & Post Methods for Create Person
        /// <summary>
        /// This action method is used to render the Create view for adding a new person. So, GET method is used.
        /// </summary>
        /// <returns></returns>
        [Route("create")]
        [HttpGet]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "Create_Method-Custom-key", "Custom-value", 1 }, Order = 1)] // here order set which filter to execute first like method or class level
        public async Task<IActionResult> Create()
        {
            // This action method is used to render the Create view for adding a new person
            List<CountryResponse> countryList = await _countriesService.GetAllCountries();
            ViewBag.Countries = countryList;
            return View();
        }

        /// <summary>
        /// This action method is used to handle the form submission for creating a new person. So, POST method is used.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))] // validation or other pre-operations is done here
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            // If the model state is valid, add the person using the service
            PersonResponse newPerson = await _personsService.AddPerson(personAddRequest);
            // After adding the person, redirect to the Index action method to display the updated list of persons
            return RedirectToAction("Index", "Persons");
        }
        #endregion

        #region Persons Report Downloads
        // Download persons list in three format
        [Route("[Action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await _personsService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons)
            {
                FileName = "PersonsReport.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
        [Route("[Action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream personsCSV = await _personsService.GetPersonsListCSV();
            return File(personsCSV.ToArray(), "application/octet-stream", "PersonsReport.csv");
        }
        [Route("[Action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream personsExcel = await _personsService.GetPersonsListExcel();
            return File(personsExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PersonsReport.xlsx");
        }
        #endregion
    }
}
