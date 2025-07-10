using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models.Entities
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public required string PersonId { get; set; }
        [MinLength(5, ErrorMessage = "Full name must be at least 5 characters long.")]
        public required string FullName { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
    }
}