using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Contacts_Manager_CRUD.Controllers
{
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
        /// Index action method for the PersonsController.
        /// </summary>
        /// <returns></returns>
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string searchValue,
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
            List<PersonResponse> allPersons = _personsService.GetAllPersons();
            return View(allPersons);

            // based on the search criteria, filtered persons will be returned
            // and if no search criteria is provided, all persons will be returned
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchValue);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchValue = searchValue;

            // Code for sorting persons based on the sort criteria
            List<PersonResponse>? sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(sortedPersons); // Return the view with the list of persons at Views/Persons/Index.cshtml
        }

        #region Get & Post Methods for Create Person
        /// <summary>
        /// This action method is used to render the Create view for adding a new person. So, GET method is used.
        /// </summary>
        /// <returns></returns>
        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            // This action method is used to render the Create view for adding a new person
            List<CountryResponse> countryList = _countriesService.GetAllCountries();
            ViewBag.Countries = countryList;
            return View();
        }

        /// <summary>
        /// This action method is used to handle the form submission for creating a new person. So, POST method is used.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [Route("persons/create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest person)
        {
            // This action method is used to handle the form submission for creating a new person
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countryList = _countriesService.GetAllCountries();
                ViewBag.Countries = countryList;
                ViewBag.Errors = ModelState.Values.SelectMany(error =>  error.Errors)
                    .Select(errorMessages => errorMessages.ErrorMessage).ToList();
                return View();
            }

            // If the model state is valid, add the person using the service
            PersonResponse newPerson = _personsService.AddPerson(person);
            // After adding the person, redirect to the Index action method to display the updated list of persons
            return RedirectToAction("Index", "Persons");
        }
        #endregion
    }
}
