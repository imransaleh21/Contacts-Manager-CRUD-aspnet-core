using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly ICountriesService _countriesService;
        public PersonsService(/*ICountriesService countriesService*/)
        {
            _countriesService = new CountriesService();
        }

        public PersonResponse AddPerson(PersonAddRequest? addPerson)
        {
            if(addPerson == null) throw new ArgumentNullException(nameof(addPerson));
            else if(addPerson.PersonName == null) throw new ArgumentException(nameof(addPerson.PersonName));
            else if(addPerson.Email == null) throw new ArgumentException(nameof(addPerson.Email));
            else if(addPerson.ReceiveNewsLettter == null) throw new ArgumentException(nameof(addPerson.ReceiveNewsLettter));

            //Convert the PersonAddRequest DTO object into Person object
            Person person = addPerson.ToPerson();
            person.PersonId = Guid.NewGuid();

            //Now convert the person object into Person Response DTO type
            PersonResponse personResponse = person.ToPersonResponse();

            if(addPerson.CountryId != null)
            {
                CountryResponse? countryResponse = _countriesService.GetCountryByCountryId(person.CountryId);
                if (countryResponse != null) personResponse.Country = countryResponse.CountryName;
            }
            return personResponse;
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }
    }
}
