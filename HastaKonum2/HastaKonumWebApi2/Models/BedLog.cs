using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HastaKonumWebApi2.Models
{
    public class BedLog
    {
        [Key]
        public int Id { get; set; }

        public int BedId { get; set; }

        [JsonIgnore]
        public Bed? Bed { get; set; }

        public string Action { get; set; } // Örnek: "Dolu yapıldı", "Boş bırakıldı", "Temizlik başlatıldı"

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public int? UserId { get; set; } // Opsiyonel olarak işlemi yapan kullanıcı
        [JsonIgnore]
        public Users? User { get; set; }
    }
}
