using System.Globalization;
using System.Reflection;
using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
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
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsService> _logger;

        public PersonsService(IPersonsRepository personsRepository, ILogger<PersonsService> logger)
        {
            _personsRepository = personsRepository;
            _logger = logger;
        }
        /// <summary>
        /// Adds a new person object to the list of persons.
        /// </summary>
        /// <param name="addPerson"></param>
        /// <returns> the added person will be returned here </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PersonResponse> AddPerson(PersonAddRequest? addPerson)
        {
            _logger.LogInformation("AddPerson method");

            if (addPerson == null) throw new ArgumentNullException(nameof(addPerson));
            // Add model validation for any validation error
            ValidationHelper.ValidateTheModelObject(addPerson);

            //Convert the PersonAddRequest DTO object into Person object then add it to the list of persons
            Person person = addPerson.ToPerson();
            person.PersonId = Guid.NewGuid();

            // Add the person to the Persons DbSet and save changes
            await _personsRepository.AddPerson(person);

            // Using stored procedure to insert person
            // But this create a problem for mocking the DbContext in unit tests
            //await _db.sp_InsertPerson(person);

            //Now convert the person object into Person Response DTO type and fetch the country name if available
            return person.ToPersonResponse();
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
        /// This method will sort the persons based on the sort criteria provided.
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="sortBy">The fileld to be sorted</param>
        /// <param name="sortOrder">Which order you want to sort(ASC or DESC)</param>
        /// <returns>The sorted person list</returns>
        public List<PersonResponse>? GetSortedPersons(List<PersonResponse> personList, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return personList;

            // Use a switch expression to sort the personList based on the sortBy and sortOrder parameters
            //List<PersonResponse> sortedPersonsList = (sortBy, sortOrder)
            //    switch
            //    {
            //        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
            //        => personList.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Email), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Email), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.DateOfBirth).ToList(),

            //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.DateOfBirth).ToList(),

            //        (nameof(PersonResponse.Age), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Age).ToList(),

            //        (nameof(PersonResponse.Age), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Age).ToList(),

            //        (nameof(PersonResponse.Country), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Country), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.ReceiveNewsLettter).ToList(),

            //        (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.ReceiveNewsLettter).ToList(),

            //        _ => personList // Default case, return the original list if no valid sortBy is provided
            //    };

            /*
            The same switch case expression is replaced with
            Reflection to sort the personList based on the sortBy and sortOrder parameters
            ✅ More maintainable(no repetitive switch cases)
            ✅ Dynamic(works even if you add new sortable properties later
            */
            // Get the property info for the sortBy Field
            var sortByProperty = typeof(PersonResponse).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (sortByProperty == null) return personList;

            // Check if the property type is string
            bool isStringProperty = sortByProperty.PropertyType == typeof(string);

            IEnumerable<PersonResponse> sortedPersonsList;

            if (sortOrder == SortOrderOptions.ASC)
            {
                if (isStringProperty)
                {
                    sortedPersonsList = personList.OrderBy(p => sortByProperty.GetValue(p) as string ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    sortedPersonsList = personList.OrderBy(p => sortByProperty.GetValue(p));
                }
            }
            else
            {
                if (isStringProperty)
                {
                    sortedPersonsList = personList.OrderByDescending(p => sortByProperty.GetValue(p) as string ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    sortedPersonsList = personList.OrderByDescending(p => sortByProperty.GetValue(p));
                }
            }
            return sortedPersonsList.ToList();
        }

        /// <summary>
        /// This method will update the existing person details based on the provided update request.
        /// </summary>
        /// <param name="personUpdateRequest">Person Details to be Updated</param>
        /// <returns>After updating the person based on updated info this function will return PersonResponse obj</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            // check if the personUpdate is null
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));

            // Add model validation for any validation error
            ValidationHelper.ValidateTheModelObject(personUpdateRequest);

            // Get the person details by Person ID that need to be updated 
            Person? matchedPerson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (matchedPerson == null) throw new InvalidPersonIdException("Invalid person id");

            // Update the matchedPerson details based on the personUpdateRequest details
            Person personToUpdate = personUpdateRequest.ToPerson();
            Person updatedPerson = await _personsRepository.UpdatePerson(personToUpdate);
            // Convert the person object to PersonResponse object and return
            return updatedPerson.ToPersonResponse();
        }

        /// <summary>
        /// This method will delete a person based on the provided personId.
        /// </summary>
        /// <param name="personId">Person Id of the person who has to be deleted</param>
        /// <returns>True if deletion is successful, otherwise false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> DeletePerson(Guid? personId)
        {
            // Check if the personId is null
            if (personId == null) throw new ArgumentNullException(nameof(personId));

            PersonResponse? personToRemove = await GetPersonByPersonId(personId.Value);
            if (personToRemove == null) return false;

            // If the person is found, remove it from the list of persons
            await _personsRepository.DeletePersonByPersonID(personId.Value);
            return true;
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
