using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly PersonsDbContext _db;
        public PersonsRepository(PersonsDbContext db)
        {
            _db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personId)
        {
            _db.Persons.Remove(_db.Persons.FirstOrDefault(p => p.PersonId == personId)!);
            return await _db.SaveChangesAsync().ContinueWith(task => task.Result > 0);
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country")
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonId(Guid? personId)
        {
            return await _db.Persons.Include("Country")
                .FirstOrDefaultAsync(p => p.PersonId == personId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchedPerson = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
            _db.Persons.Update(person);
            if(matchedPerson == null) return person;

            matchedPerson.PersonName = person.PersonName;
            matchedPerson.Email = person.Email;
            matchedPerson.DateOfBirth = person.DateOfBirth;
            matchedPerson.Gender = person.Gender;
            matchedPerson.CountryId = person.CountryId;
            matchedPerson.Address = person.Address;
            matchedPerson.ReceiveNewsLettter = person.ReceiveNewsLettter;
            matchedPerson.PIN = person.PIN;

            await _db.SaveChangesAsync();
            return matchedPerson;
        }
    }
}
