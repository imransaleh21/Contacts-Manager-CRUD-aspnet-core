using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents Data access logic for Person entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a new Person to the data source
        /// </summary>
        /// <param name="person">Person Obj to add</param>
        /// <returns>Added person after adding it to the table</returns>
        Task<Person> AddPerson(Person person);
        /// <summary>
        /// Gets all Persons from the data source
        /// </summary>
        /// <returns>List of person object from table</returns>
        Task<List<Person>> GetAllPersons();
        /// <summary>
        /// Gets a Person by personId from the data source
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Return the matched person or null for unmatch</returns>
        Task<Person?> GetPersonByPersonId(Guid? personId);
        /// <summary>
        /// Get filtered persons based on the given expression predicate
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>Matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);
        /// <summary>
        /// Updates a person in the data source
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Updated person object</returns>
        Task<Person> UpdatePerson(Person person);
        /// <summary>
        /// Deletes a person by personId from the data source
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>bool value based on successful or not</returns>
        Task<bool> DeletePersonByPersonID(Guid personId);
    }
}
