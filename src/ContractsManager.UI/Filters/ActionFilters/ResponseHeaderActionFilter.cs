using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager_CRUD.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        private readonly string key;
        private readonly string value;
        //public int Order { get; set; } // To set the order of filter execution
        public ResponseHeaderActionFilter(string key, string value, int order)
        {
            this.key = key;
            this.value = value;
            Order = order;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        { 
            context.HttpContext.Response.Headers[key] = value;
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{

        //}
    }
}
