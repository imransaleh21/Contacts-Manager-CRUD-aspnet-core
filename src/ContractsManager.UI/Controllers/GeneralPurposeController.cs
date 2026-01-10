using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Contacts_Manager_CRUD.Controllers
{
    public class GeneralPurposeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature? exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if(exceptionHandlerFeature?.Error is not null)
            {
                ViewBag.ErrorMessage = exceptionHandlerFeature.Error.Message;
            }
            return View();
        }
    }
}
