using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class PersonsGetterServiceReducedExcelColumns : IPersonsGetterService
    {
        private readonly PersonsGetterService _personsGetterService;
        public PersonsGetterServiceReducedExcelColumns(PersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personsGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchValue)
        {
            return await _personsGetterService.GetFilteredPersons(searchBy, searchValue);
        }

        public Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            return _personsGetterService.GetPersonByPersonId(personId);
        }

        public Task<MemoryStream> GetPersonsListCSV()
        {
            return _personsGetterService.GetPersonsListCSV();
        }

        public async Task<MemoryStream> GetPersonsListExcel()
        {
            MemoryStream memoryStream = new();
            using (ExcelPackage excelPackage = new(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsList");
                // Add header row
                worksheet.Cells[1, 1].Value = "PersonName";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "DateOfBirth";
                worksheet.Cells[1, 4].Value = "Age";
                // Add data rows
                List<PersonResponse> allPersons = await GetAllPersons();
                for (int i = 0; i < allPersons.Count; i++)
                {
                    PersonResponse person = allPersons[i];
                    worksheet.Cells[i + 2, 1].Value = person.PersonName;
                    worksheet.Cells[i + 2, 2].Value = person.Email;
                    worksheet.Cells[i + 2, 3].Value = person.DateOfBirth?.ToString("dd MMMM yyyy");
                    worksheet.Cells[i + 2, 4].Value = person.Age;
                }
                // this auto fit will adjust the width of all columns based on their content
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                await excelPackage.SaveAsAsync(memoryStream);
            }
            memoryStream.Position = 0; // Reset the memory stream position to the beginning
            return memoryStream;
        }
    }
}
