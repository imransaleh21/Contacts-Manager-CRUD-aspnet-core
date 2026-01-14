using Contacts_Manager_CRUD.Controllers;
using ContactsManager.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class RegisterPostActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is AccountController accountController)
            {
                if(!accountController.ModelState.IsValid)
                {
                    accountController.ViewBag.Errors = accountController.ModelState.Values.SelectMany(error => error.Errors)
                        .Select(errorMessages => errorMessages.ErrorMessage).ToList();

                    // Get the first parameter(DTO) dynamically
                    var registerReq = context.ActionArguments.Values.FirstOrDefault();
                    context.Result = accountController.View(registerReq);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
