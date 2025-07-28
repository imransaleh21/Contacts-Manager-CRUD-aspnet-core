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
        public IActionResult Index()
        {
            // Retrieve all persons from the service
            List<PersonResponse> persons = _personsService.GetAllPersons();
            return View(persons); // Return the view with the list of persons at Views/Persons/Index.cshtml
        }
    }
}
