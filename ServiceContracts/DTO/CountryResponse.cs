using System;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class to return response from most of the CountryServices
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }

    public static class CountryExtensions 
    {
        // extension method of for Country class
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
