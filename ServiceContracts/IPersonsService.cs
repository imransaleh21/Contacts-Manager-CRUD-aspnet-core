using ServiceContracts.DTO;
using System;

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

    }
}
