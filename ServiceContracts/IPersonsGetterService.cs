using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// IPersonsService interface defines the contract for person-related operations.
    /// </summary>
    public interface IPersonsGetterService
    {
        /// <summary>
        /// By default, this method returns all persons in the list.
        /// </summary>
        /// <returns>Full list of the persons details of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// This method will get a personId and send the specific person's details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If the Id is valid then return the person's details otherwise send a proper message</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// This method will filter the persons based on the search criteria provided,
        /// the field to search by and the value to search for.
        /// </summary>
        /// <param name="searchBy">Field to search</param>
        /// <param name="searchValue">Value string for search</param>
        /// <returns>the filtered persons list after filter with specific value string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchValue);

        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns>Returns the memory stream with CSV</returns>
        Task<MemoryStream> GetPersonsListCSV();

        /// <summary>
        /// Returns persons as Excel
        /// </summary>
        /// <returns>Memory stream of Excel data</returns>
        Task<MemoryStream> GetPersonsListExcel();
    }
}
