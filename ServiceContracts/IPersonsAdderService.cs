using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// IPersonsService interface defines the contract for person-related operations.
    /// </summary>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a new person object to the list of persons.
        /// </summary>
        /// <param name="personAdd"></param>
        /// <returns> The same person details, along with newly generated personId</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? addPerson);
    }
}
