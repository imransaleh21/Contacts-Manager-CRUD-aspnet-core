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
    public class PersonsDeleterService : IPersonsDeleterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        public PersonsDeleterService(IPersonsRepository personsRepository, ILogger<PersonsDeleterService> logger)
        {
            _personsRepository = personsRepository;
            _logger = logger;
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

            Person? personToRemove = await _personsRepository.GetPersonByPersonId(personId.Value);
            if (personToRemove == null) return false;

            // If the person is found, remove it from the list of persons
            await _personsRepository.DeletePersonByPersonID(personId.Value);
            return true;
        }
    }
}
