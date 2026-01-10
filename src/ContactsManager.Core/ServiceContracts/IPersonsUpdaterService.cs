using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// IPersonsService interface defines the contract for person-related operations.
    /// </summary>
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// This method will update the existing person details based on the provided update request.
        /// </summary>
        /// <param name="personUpdate">Person Details to be Updated</param>
        /// <returns>Updated person info</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdate);
    }
}
