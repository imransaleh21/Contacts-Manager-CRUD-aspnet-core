using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager_CRUD.Filters.ActionFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;
        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before executing the action result.", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

            await next();

            _logger.LogInformation("{FilterName}.{MethodName} - After executing the action result.", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));
            context.HttpContext.Response.Headers.LastModified = DateTime.Now.ToString("yyyyy-MM-dd");
        }
    }
}
