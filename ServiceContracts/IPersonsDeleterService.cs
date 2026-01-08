using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// IPersonsService interface defines the contract for person-related operations.
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// This method will delete a person based on the provided personId.
        /// </summary>
        /// <param name="personId">Person Id of the person who has to be deleted</param>
        /// <returns>True if deletion is successful, otherwise false</returns>
        Task<bool> DeletePerson(Guid? personId);
    }
}
