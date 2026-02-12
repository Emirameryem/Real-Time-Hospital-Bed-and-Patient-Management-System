using System.ComponentModel.DataAnnotations;

namespace HastaKonumWebApi2.Models
{
    public class Bed
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BedNumber { get; set; } // Yatak numarası (örnek: A101)

        [Required]
        public string Status { get; set; } // "Boş", "Dolu", "Temizlikte"

        public ICollection<BedLog> Logs { get; set; } // Yatak geçmişi için log ilişkisi
    }
}
