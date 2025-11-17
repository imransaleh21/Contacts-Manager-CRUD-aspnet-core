using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace Contacts_Manager_CRUD.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select a valid Excel file.";
                return View();
            }
            if (Path.GetExtension(excelFile.FileName).ToLower() != ".xlsx")
            {
                ViewBag.ErrorMessage = "Only .xlsx files are supported.";
                return View();
            }
            try
            {
                int addedCountriesCount = await _countriesService.UploadContriesFromExcelFile(excelFile);
                ViewBag.SuccessMessage = $"{addedCountriesCount} countries have been added successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }
    }
}
