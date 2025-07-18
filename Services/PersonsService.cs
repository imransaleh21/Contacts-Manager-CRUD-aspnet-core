using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
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
    }
}
