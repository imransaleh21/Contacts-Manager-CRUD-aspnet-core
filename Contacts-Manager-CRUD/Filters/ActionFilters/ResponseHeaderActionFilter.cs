using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager_CRUD.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IActionFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string key;
        private readonly string value;
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
        {
            _logger = logger;
            this.key = key;
            this.value = value;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} called", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuted));
            context.HttpContext.Response.Headers[key] = value;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} called", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuting));
        }
    }
}
