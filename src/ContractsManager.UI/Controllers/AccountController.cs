using Contacts_Manager_CRUD.Controllers;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.IdentityContracts;
using ContactsManager.UI.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IRegisterService _registerService;
        //private readonly ISignInService _signInService;
        public AccountController(IRegisterService registerService)
        {
            _registerService = registerService;
            //_signInService = signInService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(RegisterPostActionFilter))]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            Result<Guid> registerResult = await _registerService.RegisterUser(registerDTO);
            if (registerResult.IsSuccess)
            {
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (string error in registerResult.Errors)
                {
                    ModelState.AddModelError("Register", error);
                }
                return View(registerDTO);
            }
            
        }
    }
}
