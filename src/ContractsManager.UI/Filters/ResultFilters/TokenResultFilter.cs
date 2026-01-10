using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager_CRUD.Filters.ResultFilters
{
    public class TokenResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Cookies.Append("Auth-Key", "A10X");
            await next();
        }
    }
}
