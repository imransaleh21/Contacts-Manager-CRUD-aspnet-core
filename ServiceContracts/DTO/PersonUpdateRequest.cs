using Entities;
using ServiceContracts.Enums;
using ServiceContracts.Validations;
using System.ComponentModel.DataAnnotations;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// This DTO is used for updating an existing person
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [DateChecker]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public bool? ReceiveNewsLettter { get; set; }

        //Convert the PersonAddRequest Object to Person Object
        public Person ToPerson()
        {
            return new()
            {
                PersonId = PersonId,
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
