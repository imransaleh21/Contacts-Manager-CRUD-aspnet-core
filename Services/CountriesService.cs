using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext dbContext)
        {
            _db = dbContext;
        }
        /// <summary>
        /// Adds a new country to the database
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            // validation: countryAddRequest should not be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            // validation: countryAddRequest.CountryName should not be null
            else if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            // validation: Duplicate country name should not be allowed
            else if (await _db.Countries
                .Where(country => country.CountryName == countryAddRequest.CountryName)
                .CountAsync() > 0)
            {
                throw new ArgumentException($"Country with name {countryAddRequest.CountryName} already exists.");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            // Add country to Countries DbSet and save changes
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        /// <summary>
        /// Gets all countries from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _db.Countries
                .Select(country => country.ToCountryResponse())
                .ToListAsync();
        }

        /// <summary>
        /// Gets a country by countryId
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                return null; // If countryId is null, return null
            }
            else
            {
                Country? country = await _db.Countries
                    .FirstOrDefaultAsync(country => country.CountryID == countryId);
                return country?.ToCountryResponse() ?? null;
            }
        }

        /// <summary>
        /// Uploads countries from an Excel file
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns>Number of Inserted countries</returns>
        public async Task<int> UploadContriesFromExcelFile(IFormFile excelFile)
        {
            int insertedCount = 0;
            MemoryStream memoryStream = new();
            await excelFile.CopyToAsync(memoryStream);
            using (ExcelPackage excelPackage = new(memoryStream))
            {
                ExcelWorksheet? workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Countries")
                    ?? throw new ArgumentException("The worksheet must be named with 'Countries' in the Excel file.");

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string countryName = workSheet.Cells[row, 1].Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(countryName))
                    {
                        // Check for duplicate country names
                        bool countryExists = await _db.Countries
                            .AnyAsync(country => country.CountryName == countryName);
                        if (!countryExists)
                        {
                            Country country = new()
                            {
                                CountryID = Guid.NewGuid(),
                                CountryName = countryName
                            };
                            _db.Countries.Add(country);
                            await _db.SaveChangesAsync();
                            insertedCount++;
                        }
                    }
                }
            }
            return insertedCount;
        }
    }
}
