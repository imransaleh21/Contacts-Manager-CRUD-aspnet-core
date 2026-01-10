using System.Globalization;
using System.Reflection;
using CsvHelper;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;

        public PersonsGetterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger)
        {
            _personsRepository = personsRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// By default, this method will convert the Person list to PersonResponse list and returns all persons details.
        /// </summary>
        /// <returns>PersonResponse details</returns>
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var person = await _personsRepository.GetAllPersons();
            return person
                .Select(person => person.ToPersonResponse()).ToList();

            // Using stored procedure to get all persons,
            // now we don't need to call .ToList() to make list on memory every time
            //return _db.sp_GetAllPersons()
            //    .Select(person => PersonToPersonResponseWithCountry(person)).ToList();
        }

        /// <summary>
        /// Send the person details of the corresponding person ID, if the id is null then return null
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Return person details</returns>
        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            _logger.LogInformation($"GetPersonByPersonId method Param: {personId}");
            if (personId == null) return null;
            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

            return person?.ToPersonResponse() ?? null;
        }

        /// <summary>
        /// This method will filter the persons based on the search criteria provided. The first parameter is the field to search by,
        /// and the second parameter is the value to search for.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>It return the list of PersonResponse</returns>
        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchValue)
        {
            List<Person> matchingPersons = new();
            using (Operation.Time("Time to Get Filtered Persons"))
            {
                // For DateOfBirth, we need to filter in-memory since ToString() can't be translated
                if (searchBy == nameof(PersonResponse.DateOfBirth))
                {
                    var allPersons = await _personsRepository.GetAllPersons();
                    matchingPersons = allPersons
                        .Where(person => person.DateOfBirth.HasValue &&
                               person.DateOfBirth.Value.ToString("dd MMMM yy").Contains(searchValue ?? string.Empty))
                        .ToList();
                }
                else
                {
                    matchingPersons = searchBy switch
                    {
                        nameof(PersonResponse.PersonName) => await _personsRepository.GetFilteredPersons(person =>
                            person.PersonName.Contains(searchValue)),

                        nameof(PersonResponse.Email) => await _personsRepository.GetFilteredPersons(person =>
                            person.Email.Contains(searchValue)),

                        nameof(PersonResponse.Gender) => await _personsRepository.GetFilteredPersons(person =>
                            person.Gender.Contains(searchValue)),

                        nameof(PersonResponse.CountryId) => await _personsRepository.GetFilteredPersons(person =>
                            person.Country.CountryName.Contains(searchValue)),

                        nameof(PersonResponse.Address) => await _personsRepository.GetFilteredPersons(person =>
                            person.Address.Contains(searchValue)),

                        _ => await _personsRepository.GetAllPersons()
                    };
                }
            }
            return matchingPersons.Select(p => p.ToPersonResponse()).ToList();
        }
        /// <summary>
        /// This method will generate a CSV file containing the list of all persons.
        /// </summary>
        /// <returns>memoryStream of the list</returns>
        public async Task<MemoryStream> GetPersonsListCSV()
        {
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);
            CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            // Below WriteHeader and WriteRecords write everything from the response,
            // If we want to write specific raw then we can use WriteField (ref v.: 18/237)
            csvWriter.WriteHeader<PersonResponse>(); // Write the header row
            csvWriter.NextRecord();
            List<PersonResponse> allPersons = await GetAllPersons();

            await csvWriter.WriteRecordsAsync(allPersons); // Write all person records asynchronously
            memoryStream.Position = 0; // Reset the memory stream position to the beginning
            return memoryStream;
        }
        /// <summary>
        /// This method will generate an Excel file containing the list of all persons.
        /// </summary>
        /// <returns>Memory stream of person list in Excel format</returns>
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
                worksheet.Cells[1, 5].Value = "Gender";
                worksheet.Cells[1, 6].Value = "Country";
                worksheet.Cells[1, 7].Value = "Address";
                worksheet.Cells[1, 8].Value = "ReceiveNewsLettter";
                // Add data rows
                List<PersonResponse> allPersons = await GetAllPersons();
                for (int i = 0; i < allPersons.Count; i++)
                {
                    PersonResponse person = allPersons[i];
                    worksheet.Cells[i + 2, 1].Value = person.PersonName;
                    worksheet.Cells[i + 2, 2].Value = person.Email;
                    worksheet.Cells[i + 2, 3].Value = person.DateOfBirth?.ToString("dd MMMM yyyy");
                    worksheet.Cells[i + 2, 4].Value = person.Age;
                    worksheet.Cells[i + 2, 5].Value = person.Gender;
                    worksheet.Cells[i + 2, 6].Value = person.Country;
                    worksheet.Cells[i + 2, 7].Value = person.Address;
                    worksheet.Cells[i + 2, 8].Value = person.ReceiveNewsLettter;
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
