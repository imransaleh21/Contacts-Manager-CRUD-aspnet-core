using Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new country.
    /// This is used in the Country Service methods for adding a new country.
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry() { 
            return new Country() { CountryName = CountryName }; 
        }
    }
}
