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
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService> _logger;

        public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsAdderService> logger)
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
    }
}
