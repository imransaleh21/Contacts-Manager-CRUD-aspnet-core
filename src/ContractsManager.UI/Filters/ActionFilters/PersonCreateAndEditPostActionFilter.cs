using Contacts_Manager_CRUD.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Contacts_Manager_CRUD.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;
        private readonly ICountriesService _countriesService;

        public PersonCreateAndEditPostActionFilter(ILogger<PersonCreateAndEditPostActionFilter> logger, ICountriesService countriesService)
        {
            _logger = logger;
            _countriesService = countriesService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} before called", nameof(PersonCreateAndEditPostActionFilter), nameof(OnActionExecutionAsync));
            // Before executing logic
            if (context.Controller is PersonsController personsController)
            {
                // This action method is used to handle the form submission for creating a new person
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countryList = await _countriesService.GetAllCountries();
                    personsController.ViewBag.Countries = countryList;
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(error => error.Errors)
                        .Select(errorMessages => errorMessages.ErrorMessage).ToList();

                    // Get the first parameter dynamically (works for both Create and Edit)
                    var personRequest = context.ActionArguments.Values.FirstOrDefault();
                    context.Result = personsController.View(personRequest);
                }
                else
                {
                    // Proceed to the next subsequent action filter or action method in the pipeline
                    await next();
                    _logger.LogInformation("{FilterName}.{MethodName} - after called", nameof(PersonCreateAndEditPostActionFilter), nameof(OnActionExecutionAsync));
                    // executed logic here.(After logic)
                }
            }
            else
            {
                // Proceed to the next subsequent action filter or action method in the pipeline
                await next();
                _logger.LogInformation("{FilterName}.{MethodName} - after called", nameof(PersonCreateAndEditPostActionFilter), nameof(OnActionExecutionAsync));
                // executed logic here.(After logic)
            }
        }
    }
}
