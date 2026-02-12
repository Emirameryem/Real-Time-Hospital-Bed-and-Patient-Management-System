using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HastaKonumWebApi2.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Hasta adı

        public int BedId { get; set; } // Atandığı yatak ID’si

        [ForeignKey("BedId")]
        public Bed Bed { get; set; } // Navigation property
    }
}
