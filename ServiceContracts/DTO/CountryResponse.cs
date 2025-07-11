using System;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for representing a country response of most of the Country Service methods
    /// </summary>
    public class CountryResponse : IEquatable<CountryResponse>
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        // The Equals method is an implementation of the IEquatable<CountryResponse> interface method. 
        // It is used to determine whether the current CountryResponse object is equal to another CountryResponse object 
        // by comparing their CountryID and CountryName properties.
        public bool Equals(CountryResponse? countryToCompare)
        {
            if (countryToCompare is null)
                return false;

            return CountryID == countryToCompare.CountryID
                   && CountryName == countryToCompare.CountryName;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current CountryResponse instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current CountryResponse.</param>
        /// <returns>true if the specified object is a CountryResponse and has the same CountryID and CountryName; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as CountryResponse); // Use the Equals method defined above
        }

        /// <summary>
        /// Returns a hash code for the current CountryResponse instance.
        /// </summary>
        /// <returns>A hash code based on the CountryID and CountryName properties.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(CountryID, CountryName);
        }
    }


    public static class CountryExtensions 
    {
        // extension method of for Country class that converts it to CountryResponse
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }

}
