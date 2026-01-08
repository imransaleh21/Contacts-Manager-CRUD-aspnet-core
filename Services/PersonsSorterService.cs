using System.Globalization;
using System.Reflection;
using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        public PersonsSorterService(IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger)
        {
            _personsRepository = personsRepository;
            _logger = logger;
        }
        /// <summary>
        /// This method will sort the persons based on the sort criteria provided.
        /// </summary>
        /// <param name="personList"></param>
        /// <param name="sortBy">The fileld to be sorted</param>
        /// <param name="sortOrder">Which order you want to sort(ASC or DESC)</param>
        /// <returns>The sorted person list</returns>
        public List<PersonResponse>? GetSortedPersons(List<PersonResponse> personList, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return personList;

            // Use a switch expression to sort the personList based on the sortBy and sortOrder parameters
            //List<PersonResponse> sortedPersonsList = (sortBy, sortOrder)
            //    switch
            //    {
            //        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
            //        => personList.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Email), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Email), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.DateOfBirth).ToList(),

            //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.DateOfBirth).ToList(),

            //        (nameof(PersonResponse.Age), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Age).ToList(),

            //        (nameof(PersonResponse.Age), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Age).ToList(),

            //        (nameof(PersonResponse.Country), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Country), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            //        (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.ASC)
            //        => personList.OrderBy(person => person.ReceiveNewsLettter).ToList(),

            //        (nameof(PersonResponse.ReceiveNewsLettter), SortOrderOptions.DESC)
            //        => personList.OrderByDescending(person => person.ReceiveNewsLettter).ToList(),

            //        _ => personList // Default case, return the original list if no valid sortBy is provided
            //    };

            /*
            The same switch case expression is replaced with
            Reflection to sort the personList based on the sortBy and sortOrder parameters
            ✅ More maintainable(no repetitive switch cases)
            ✅ Dynamic(works even if you add new sortable properties later
            */
            // Get the property info for the sortBy Field
            var sortByProperty = typeof(PersonResponse).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (sortByProperty == null) return personList;

            // Check if the property type is string
            bool isStringProperty = sortByProperty.PropertyType == typeof(string);

            IEnumerable<PersonResponse> sortedPersonsList;

            if (sortOrder == SortOrderOptions.ASC)
            {
                if (isStringProperty)
                {
                    sortedPersonsList = personList.OrderBy(p => sortByProperty.GetValue(p) as string ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    sortedPersonsList = personList.OrderBy(p => sortByProperty.GetValue(p));
                }
            }
            else
            {
                if (isStringProperty)
                {
                    sortedPersonsList = personList.OrderByDescending(p => sortByProperty.GetValue(p) as string ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    sortedPersonsList = personList.OrderByDescending(p => sortByProperty.GetValue(p));
                }
            }
            return sortedPersonsList.ToList();
        }
    }
}
