using System;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    internal class PersonsService : IPersonsService
    {

        public PersonResponse AddPerson(PersonAddRequest? personAdd)
        {
            throw new NotImplementedException();
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }
    }
}
