using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Exceptions;

namespace Services
{
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;

        public PersonsUpdaterService(IPersonsRepository personsRepository, ILogger<PersonsUpdaterService> logger)
        {
            _personsRepository = personsRepository;
            _logger = logger;
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
    }
}
