using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents Data access logic for Country entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a country object to the data source
        /// </summary>
        /// <param name="country">Country obj to add</param>
        /// <returns>Added country response after adding it to data source</returns>
        Task<Country> AddCountry(Country country);
        /// <summary>
        /// Retrieves all countries from the data source
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();
        /// <summary>
        /// Retrieves a country by its unique identifier from the data source
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>Matching Country or null</returns>
        Task<Country?> GetCountryByCountryId(Guid? countryId);
        /// <summary>
        /// Retrieves a country by its name from the data source
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>Matched country or null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
