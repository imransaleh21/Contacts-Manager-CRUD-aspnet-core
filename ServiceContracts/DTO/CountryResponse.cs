using System;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for representing a country response of most of the Country Service methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
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
