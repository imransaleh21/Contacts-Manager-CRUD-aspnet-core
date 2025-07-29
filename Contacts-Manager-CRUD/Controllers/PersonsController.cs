using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Contacts_Manager_CRUD.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }
        /// <summary>
        /// Index action method for the PersonsController.
        /// </summary>
        /// <returns></returns>
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string searchValue)
        {
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

            // based on the search criteria, filtered persons will be returned
            // and if no search criteria is provided, all persons will be returned
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchValue);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchValue = searchValue;
            return View(persons); // Return the view with the list of persons at Views/Persons/Index.cshtml
        }
    }
}
