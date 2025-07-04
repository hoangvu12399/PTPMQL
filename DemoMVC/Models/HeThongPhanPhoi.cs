using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    [Table("HeThongPhanPhoi")]
    public class HeThongPhanPhoi
    {
        [Key]
        public required string MaHTPP { get; set; }
        public required string TenHTPP { get; set; }
        public List<Daily> Dailies { get; set; } = new List<Daily>();
    }
}
