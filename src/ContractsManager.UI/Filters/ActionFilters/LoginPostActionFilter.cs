using ContactsManager.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class LoginPostActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is AccountController accountController)
            {
                if (!accountController.ModelState.IsValid)
                {
                    accountController.ViewBag.Errors = accountController.ModelState.Values.SelectMany(Values => Values.Errors)
                        .Select(errorMsg => errorMsg.ErrorMessage).ToList();

                    // Get the first parameter(DTO) dynamically
                    var loginReq = context.ActionArguments.Values.FirstOrDefault();
                    context.Result = accountController.View(loginReq);
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
