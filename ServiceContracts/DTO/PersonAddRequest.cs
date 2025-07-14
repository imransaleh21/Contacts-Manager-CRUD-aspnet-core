using System;
using Entities;
using ServiceContracts.Enums;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// This DTO is used for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLettter { get; set; }

        //Convert the PersonAddRequest Object to Person Object
        public Person ToPerson()
        {
            return new()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLettter = ReceiveNewsLettter,
            };
        }
    }
}
