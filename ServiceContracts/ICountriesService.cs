using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// ICountriesService interface defines the contract for country-related operations.
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a new country object to the list of countries.
        /// </summary>
        /// <param name="countryAddRequest">Country object to be added</param>
        /// <returns>Country object after adding it with country id</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// By default, this method returns all countries in the list.
        /// </summary>
        /// <returns>All Countries in the list</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Gets a country by its unique identifier.
        /// </summary>
        /// <param name="countryId">The unique identifier used to search for a country.</param>
        /// <returns>Returns the appropriate Country with the specified ID.</returns>
        Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);
    }
}
