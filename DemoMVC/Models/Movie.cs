using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [DataType(DataType.Date)]
        public string? Genre { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Price { get; set; }
    }
}