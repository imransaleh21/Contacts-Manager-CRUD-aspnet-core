using System.Reflection;
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
        /// <param name="personId"></param>
        /// <returns>Return person details</returns>
        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if(personId == null) return null;
            Person? person = _persons.FirstOrDefault(person => person.PersonId == personId);
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

        /// <summary>
        /// This method will sort the persons based on the sort criteria provided.
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="sortBy">The fileld to be sorted</param>
        /// <param name="sortOrder">Which order you want to sort(ASC or DESC)</param>
        /// <returns>The sorted person list</returns>
        public List<PersonResponse>? GetSortedPersons(List<PersonResponse> personList, string sortBy, SortOrderOptions sortOrder)
        {
            if(string.IsNullOrEmpty(sortBy)) return personList;

            // Use a switch expression to sort the personList based on the sortBy and sortOrder parameters
            //List<PersonResponse> sortedPersonsList = (sortBy, sortOrder)
            //    switch
            //    {
            //        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
            //        => personList.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Email), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Email), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.DateOfBirth).ToList(),

            //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.DateOfBirth).ToList(),

            //        (nameof(PersonResponse.Age), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Age).ToList(),

            //        (nameof(PersonResponse.Age), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Age).ToList(),

            //        (nameof(PersonResponse.Country), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Country), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.ReceiveNewsLettter).ToList(),

            //        (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.ReceiveNewsLettter).ToList(),

            //        _ => personList // Default case, return the original list if no valid sortBy is provided
            //    };

            /*
            The same switch case expression is replaced with
            Reflection to sort the personList based on the sortBy and sortOrder parameters
            ✅ More maintainable(no repetitive switch cases)
            ✅ Dynamic(works even if you add new sortable properties later
            */
            // Get the property info for the sortBy Field
            var sortByProperty = typeof(PersonResponse).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(sortByProperty  == null) return personList;

            // Check if the property type is string
            bool isStringProperty = sortByProperty.PropertyType == typeof(string);

            IEnumerable<PersonResponse> sortedPersonsList;

            if (sortOrder == SortOrderOptions.ASC)
            {
                if (isStringProperty)
                {
                    sortedPersonsList = personList.OrderBy(p => sortByProperty.GetValue(p) as string ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    sortedPersonsList = personList.OrderBy(p => sortByProperty.GetValue(p));
                }
            }
            else
            {
                if (isStringProperty)
                {
                    sortedPersonsList = personList.OrderByDescending(p => sortByProperty.GetValue(p) as string ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    sortedPersonsList = personList.OrderByDescending(p => sortByProperty.GetValue(p));
                }
            }
                return sortedPersonsList.ToList();
        }

        /// <summary>
        /// This method will update the existing person details based on the provided update request.
        /// </summary>
        /// <param name="personUpdateRequest">Person Details to be Updated</param>
        /// <returns>After updating the person based on updated info this function will return PersonResponse obj</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            // check if the personUpdate is null
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));

            // Add model validation for any validation error
            ValidationHelper.ValidateTheModelObject(personUpdateRequest);

            // Get the person details by Person ID that need to be updated 
            PersonResponse? person = GetPersonByPersonId(personUpdateRequest.PersonId);
            if (person == null) throw new ArgumentException("Invalid person id");

            /*
             * As person info is found(not null), So, person info of that person id is found,
             * now we can update the Person info that person
            */
            Person updatedPerson = personUpdateRequest.ToPerson();

            // Convert the person object to PersonResponse object and return
            return updatedPerson.ToPersonResponse();

        }

        /// <summary>
        /// This method will delete a person based on the provided personId.
        /// </summary>
        /// <param name="personId">Person Id of the person who has to be deleted</param>
        /// <returns>True if deletion is successful, otherwise false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool DeletePerson(Guid? personId)
        {
            // Check if the personId is null
            if (personId == null) throw new ArgumentNullException(nameof(personId));

            Person? personToRemove = _persons.FirstOrDefault(person => person.PersonId == personId);
            if (personToRemove == null) return false;
            // If the person is found, remove it from the list of persons
            return _persons.Remove(personToRemove);
        }
    }
}
