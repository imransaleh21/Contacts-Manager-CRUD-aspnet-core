using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// IPersonsService interface defines the contract for person-related operations.
    /// </summary>
    public interface IPersonsSorterService
    {
        /// <summary>
        /// This method will sort the persons based on the sort criteria provided,
        /// </summary>
        /// <param name="personList">The list of all person</param>
        /// <param name="sortBy">The fild to be sorted like: Name, email..</param>
        /// <param name="sortOrder">Ascending or Descending</param>
        /// <returns>The sorted persons list after sort by ascending or descending order</returns>
        List<PersonResponse>? GetSortedPersons(List<PersonResponse> personList, string sortBy, SortOrderOptions sortOrder);
    }
}
