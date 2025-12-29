using Contacts_Manager_CRUD.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Contacts_Manager_CRUD.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// This method is called after the action method is executed. 
        /// Here, we can modify the action result or perform any post-processing logic.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} called", nameof(PersonsListActionFilter), nameof(OnActionExecuted));
            PersonsController personsController = (PersonsController)context.Controller;
            //The arguments were stored in HttpContext.Items in OnActionExecuting method
            IDictionary<string, object?>? actionArguments = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];
            // Passing the action arguments to the ViewData for use in the view.
            if (actionArguments != null)
            {
                if(actionArguments.ContainsKey("searchBy"))
                    personsController.ViewData["CurrentSearchBy"] = Convert.ToString(actionArguments["searchBy"]);
                if (actionArguments.ContainsKey("searchValue"))
                    personsController.ViewData["CurrentSearchValue"] = Convert.ToString(actionArguments["searchValue"]);
                if (actionArguments.ContainsKey("sortBy"))
                    personsController.ViewData["CurrentSortBy"] = Convert.ToString(actionArguments["sortBy"]);
                if (actionArguments.ContainsKey("sortOrder") && actionArguments["sortOrder"] is SortOrderOptions sortOrderValue)
                    personsController.ViewData["CurrentSortOrder"] = sortOrderValue;
            }

            // Code for searching persons based on the search criteria
            personsController.ViewBag.SearchField = new Dictionary<string, string>
            {
                { nameof(PersonResponse.PersonName),  "Person Name" },
                { nameof(PersonResponse.Email), "Email"},
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender"},
                { nameof(PersonResponse.CountryId), "Country"},
                { nameof(PersonResponse.Address), "Address"}
            };
        }

        /// <summary>
        /// This method is called before the action method is executed. 
        /// Here, we can validate and modify the action parameters or perform any pre-processing logic.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} called", nameof(PersonsListActionFilter), nameof(OnActionExecuting));
            // Storing action arguments in HttpContext.Items for later use in the action method or other filters
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            // Validate the searchBy parameter
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                var searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchOptions = new List<string>
                       {
                            nameof(PersonResponse.PersonName),
                            nameof(PersonResponse.Email),
                            nameof(PersonResponse.DateOfBirth),
                            nameof(PersonResponse.Gender),
                            nameof(PersonResponse.CountryId),
                            nameof(PersonResponse.Address)
                       };
                    // Reset the searchBy to PersonName
                    if (!searchOptions.Contains(searchBy))
                    {
                        _logger.LogInformation("{FilterName}.{MethodName} - Invalid {Field}: {value}", nameof(PersonsListActionFilter), nameof(OnActionExecuted), nameof(searchBy), searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                }
            }
        }
    }
}
