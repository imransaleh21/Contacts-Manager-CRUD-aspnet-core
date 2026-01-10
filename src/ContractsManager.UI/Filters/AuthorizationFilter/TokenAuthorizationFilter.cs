using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager_CRUD.Filters.AuthorizationFilter
{
    public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
            {
                context.Result = new StatusCodeResult(401); // Unauthorized
                return;
            }
            else if (context.HttpContext.Request.Cookies["Auth-Key"] != "A10X")
            {
                context.Result = new StatusCodeResult(403); // Forbidden
                return;
            }
        }
    }
}
