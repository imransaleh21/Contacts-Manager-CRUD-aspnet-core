using Entities;
using System;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// This DTO is used for representing a person response in most of the Person Service methods.
    /// </summary>
    public class PersonResponse : IEquatable<PersonResponse>
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
        public bool ReceiveNewsLettter { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current PersonResponse instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return base.Equals(obj as PersonResponse);
        }

        /// <summary>
        /// Determines whether the specified PersonResponse is equal to the current instance.
        /// </summary>
        /// <param name="personToCompare"></param>
        /// <returns></returns>
        public bool Equals(PersonResponse? personToCompare)
        {
            if (personToCompare == null) return false;

            return PersonId == personToCompare.PersonId &&
                PersonName == personToCompare.PersonName &&
                Email == personToCompare.Email &&
                DateOfBirth == personToCompare.DateOfBirth &&
                Gender == personToCompare.Gender &&
                CountryId == personToCompare.CountryId &&
                Address == personToCompare.Address;
        }

        /// <summary>
        /// Returns a hash code for the current PersonResponse instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
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
                    : null
            };
        }
    }

}
