using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Threading.Tasks;

namespace Contacts_Manager_CRUD.Controllers
{
    [Route("[controller]")] // This is as same as [Route("persons")]
    //[Route("persons")]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService,
            ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        /// <summary>
        /// Index action method for the PersonsController. This will return the view of a list of persons based on different criteria
        /// </summary>
        /// <returns>Return the view with the list of persons</returns>
        [Route("[action]")] // This route is work same as the below one
        //[Route("index")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string searchValue,
            string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            // Code for searching persons based on the search criteria
            ViewBag.SearchField = new Dictionary<string, string>
            {
                { nameof(PersonResponse.PersonName),  "Person Name" },
                { nameof(PersonResponse.Email), "Email"},
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender"},
                { nameof(PersonResponse.CountryId), "Country"},
                { nameof(PersonResponse.Address), "Address"}
            };
            // Retrieve all persons from the service
            // as filtered person method returns all persons if no search criteria is provided
            // so this code is now commented
            //List<PersonResponse> allPersons = _personsService.GetAllPersons();
            //return View(allPersons);

            // based on the search criteria, filtered persons will be returned
            // and if no search criteria is provided, all persons will be returned
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchValue);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchValue = searchValue;

            // Code for sorting persons based on the sort criteria
            List<PersonResponse>? sortedPersons = _personsService.GetSortedPersons( persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(sortedPersons); // Return the view with the list of persons at Views/Persons/Index.cshtml
        }

        #region Get & Post Methods for Create Person
        /// <summary>
        /// This action method is used to render the Create view for adding a new person. So, GET method is used.
        /// </summary>
        /// <returns></returns>
        [Route("create")]
        [HttpGet]
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
        public async Task<IActionResult> Create(PersonAddRequest person)
        {
            // This action method is used to handle the form submission for creating a new person
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countryList = await  _countriesService.GetAllCountries();
                ViewBag.Countries = countryList;
                ViewBag.Errors = ModelState.Values.SelectMany(error =>  error.Errors)
                    .Select(errorMessages => errorMessages.ErrorMessage).ToList();
                return View();
            }

            // If the model state is valid, add the person using the service
            PersonResponse newPerson = await _personsService.AddPerson(person);
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
