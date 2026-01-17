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
        private readonly ISignInService _signInService;
        private readonly ILogOutService _logOutService;
        public AccountController(IRegisterService registerService, ISignInService signInService, ILogOutService logOutService)
        {
            _registerService = registerService;
            _signInService = signInService;
            _logOutService = logOutService;
        }
        #region Register
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
        #endregion
        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(LoginPostActionFilter))]
        public async Task<IActionResult> Login (LoginDTO loginDTO)
        {
            Result<Guid> loginResult = await _signInService.SignInUser(loginDTO);
            if(loginResult.IsSuccess) return RedirectToAction(nameof(PersonsController.Index), "Persons");
            else
            {
                foreach (string error in loginResult.Errors)
                {
                    ModelState.AddModelError("Login", error);
                }
                return View(loginDTO);
            }
        }
        #endregion
        #region Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _logOutService.LogOut();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        #endregion
    }
}
