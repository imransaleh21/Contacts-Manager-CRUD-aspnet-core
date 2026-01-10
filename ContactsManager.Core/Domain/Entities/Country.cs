using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Country
    {
        /// <summary>
        /// Domain Model for Country
        /// </summary>
        public Country()
        {
            CountryID = Guid.NewGuid();
        }
        [Key]
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

    }
}
