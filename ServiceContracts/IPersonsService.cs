using ServiceContracts.DTO;
using System;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// IPersonsService interface defines the contract for person-related operations.
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person object to the list of persons.
        /// </summary>
        /// <param name="personAdd"></param>
        /// <returns> The same person details, along with newly generated personId</returns>
        PersonResponse AddPerson(PersonAddRequest? addPerson);

        /// <summary>
        /// By default, this method returns all persons in the list.
        /// </summary>
        /// <returns>Full list of the persons details of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// This method will get a personId and send the specific person's details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If the Id is valid then return the person's details otherwise send a proper message</returns>
        PersonResponse? GetPersonByPersonId(Guid? id);

        /// <summary>
        /// This method will filter the persons based on the search criteria provided,
        /// the field to search by and the value to search for.
        /// </summary>
        /// <param name="searchBy">Field to search</param>
        /// <param name="searchValue">Value string for search</param>
        /// <returns>the filtered persons list after filter with specific value string</returns>
        List<PersonResponse>? GetFilteredPersons(string searchBy, string? searchValue);

        /// <summary>
        /// This method will sort the persons based on the sort criteria provided,
        /// </summary>
        /// <param name="personList">The list of all person</param>
        /// <param name="sortBy">The fild to be sorted like: Name, email..</param>
        /// <param name="sortOrder">Ascending or Descending</param>
        /// <returns>The sorted persons list after sort by ascending or descending order</returns>
        List<PersonResponse>? GetSortedPersons(List<PersonResponse> personList, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// This method will update the existing person details based on the provided update request.
        /// </summary>
        /// <param name="personUpdate">Person Details to be Updated</param>
        /// <returns>Updated person info</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdate);

    }
}
