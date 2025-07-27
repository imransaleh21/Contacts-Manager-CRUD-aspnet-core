using Microsoft.AspNetCore.Mvc;

namespace Contacts_Manager_CRUD.Controllers
{
    public class PersonsController : Controller
    {
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
