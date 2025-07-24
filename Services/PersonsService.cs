using System;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly ICountriesService _countriesService;
        private readonly List<Person> _persons;

        public PersonsService(ICountriesService countriesService)
        {
            _countriesService = countriesService;
            _persons = new List<Person>();
        }

        /// <summary>
        /// Converts a Person object to a PersonResponse object and fetches the country name if available.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>person response theof  person object</returns>
        private PersonResponse PersonToPersonResponseWithCountry(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();

            //If the person has a countryId, then get the country name from the countries service only if there is a country
            //with the given countryId
            if (person.CountryId != null)
            {
                personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
            }
            return personResponse;
        }

        /// <summary>
        /// Adds a new person object to the list of persons.
        /// </summary>
        /// <param name="addPerson"></param>
        /// <returns> the added person will be returned here </returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public PersonResponse AddPerson(PersonAddRequest? addPerson)
        {
            if(addPerson == null) throw new ArgumentNullException(nameof(addPerson));
            // Add model validation for any validation error
            ValidationHelper.ValidateTheModelObject(addPerson);

            //Convert the PersonAddRequest DTO object into Person object then add it to the list of persons
            Person person = addPerson.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons.Add(person);

            //Now convert the person object into Person Response DTO type and fetch the country name if available
            return PersonToPersonResponseWithCountry(person);
        }

        /// <summary>
        /// By default, this method will convert the Person list to PersonResponse list and returns all persons details.
        /// </summary>
        /// <returns>PersonResponse details</returns>
        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }

        /// <summary>
        /// Send the person details of the corresponding person ID, if the id is null then return null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return person details</returns>
        public PersonResponse? GetPersonByPersonId(Guid? id)
        {
            if(id == null) return null;
            Person? person = _persons.FirstOrDefault(person => person.PersonId == id);
            return person?.ToPersonResponse()??null;
        }

        /// <summary>
        /// This method will filter the persons based on the search criteria provided. The first parameter is the field to search by,
        /// and the second parameter is the value to search for.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>It return the list of PersonResponse</returns>
        public List<PersonResponse>? GetFilteredPersons(string searchBy, string? searchValue)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchValue)) 
                return matchingPersons;

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.PersonName)) ?
                    person.PersonName.Contains(searchValue, StringComparison.OrdinalIgnoreCase): true).ToList();
                    break;
                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Email)) ?
                    person.Email.Contains(searchValue, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(person => (person.DateOfBirth != null) ?
                    person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchValue, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Gender)) ?
                    person.Gender.Contains(searchValue, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.CountryId):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Country)) ?
                    person.Country.Contains(searchValue, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(person => (!string.IsNullOrEmpty(person.Address)) ?
                    person.Address.Contains(searchValue, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }

        public List<PersonResponse>? GetSortedPersons(List<PersonResponse> personList, string sortBy, SortOrderOptions sortOrder)
        {
            if(string.IsNullOrEmpty(sortBy)) return personList;

            // Use a switch expression to sort the personList based on the sortBy and sortOrder parameters
            List<PersonResponse> sortedPersonsList = (sortBy, sortOrder)
                switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
                    => personList.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                    => personList.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                    => personList.OrderBy(person => person.DateOfBirth).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.DateOfBirth).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => personList.OrderBy(person => person.Age).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.Age).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                    => personList.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                    => personList.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.ASC)
                    => personList.OrderBy(person => person.ReceiveNewsLettter).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.DESC)
                    => personList.OrderByDescending(person => person.ReceiveNewsLettter).ToList(),

                    _ => personList // Default case, return the original list if no valid sortBy is provided
                };
            return sortedPersonsList;
        }
    }
}
