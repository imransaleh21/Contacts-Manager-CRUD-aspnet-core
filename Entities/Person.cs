using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        [StringLength(45)]
        public string? PersonName { get; set; }
        [StringLength(30)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [StringLength(65)]
        public string? Address { get; set; }
        public bool? ReceiveNewsLettter { get; set; }

        public string? PIN { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }
    }
}
