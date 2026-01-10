using Entities;
using System;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// This DTO is used for representing a person response in most of the Person Service methods.
    /// </summary>
    public class PersonResponse //: IEquatable<PersonResponse>
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Age { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLettter { get; set; }

        #region Commented as we use mock testing for PersonService tests
        /// <summary>
        /// Determines whether the specified object is equal to the current PersonResponse instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public override bool Equals(object? obj)
        //{
        //    return Equals(obj as PersonResponse);
        //}

        /// <summary>
        /// Determines whether the specified PersonResponse is equal to the current instance.
        /// </summary>
        /// <param name="personToCompare"></param>
        /// <returns></returns>
        //public bool Equals(PersonResponse? personToCompare)
        //{
        //    if (personToCompare == null) return false;

        //    return PersonId == personToCompare.PersonId &&
        //        PersonName == personToCompare.PersonName &&
        //        Email == personToCompare.Email &&
        //        DateOfBirth == personToCompare.DateOfBirth &&
        //        Gender == personToCompare.Gender &&
        //        CountryId == personToCompare.CountryId &&
        //        Address == personToCompare.Address &&
        //        Country == personToCompare.Country &&
        //        ReceiveNewsLettter == personToCompare.ReceiveNewsLettter;
        //}

        /// <summary>
        /// Returns a hash code for the current PersonResponse instance.
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        #endregion
        /// <summary>
        /// Converts the current PersonResponse instance to a PersonUpdateRequest DTO.
        /// </summary>
        /// <returns></returns>
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions?)Enum.Parse(typeof(GenderOptions), Gender, true),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLettter = ReceiveNewsLettter
            };
        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// Extension methods for converting Person domain model to PersonResponse DTO.
        /// </summary>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLettter = person.ReceiveNewsLettter,
                // Calculate age based on DateOfBirth
                Age = person.DateOfBirth.HasValue 
                    ? $"{(DateTime.Now - person.DateOfBirth.Value).TotalDays / 365:F0} years" 
                    : null,
                Country = person.Country?.CountryName
            };
        }
    }

}
