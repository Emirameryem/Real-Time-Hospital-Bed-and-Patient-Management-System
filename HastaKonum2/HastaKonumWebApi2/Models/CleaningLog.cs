using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HastaKonumWebApi2.Models
{
    public class CleaningLog
    {
        [Key]
        public int Id { get; set; }

        public int BedId { get; set; } // Hangi yatak temizlenmiş

        [ForeignKey("BedId")]
        public Bed Bed { get; set; }

        public DateTime StartTime { get; set; } // Temizlik başlangıç zamanı
        public DateTime? EndTime { get; set; } // Temizlik bitiş zamanı (nullable)

        public int CleanerId { get; set; } // Temizliği yapan personel
        [ForeignKey("CleanerId")]
        public Users Cleaner { get; set; }
    }
}
