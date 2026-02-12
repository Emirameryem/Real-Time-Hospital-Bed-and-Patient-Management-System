using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HastaKonumWebApi2.Models
{
    public class Users
    {
        [Key] // Birincil anahtar
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } // Girişte kullanılacak kullanıcı adı

        [Required]
        [JsonIgnore]  // Şifreyi JSON dönüşünden gizle
        public string Password { get; set; } // Şifre (hashlenecek)

        [Required]
        public string Role { get; set; } // Rol: "Hemşire", "Temizlikçi", "Admin"
    }
}
