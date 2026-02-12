using Microsoft.EntityFrameworkCore;
using HastaKonumWebApi2.Models;
namespace HastaKonumWebApi2.Data
{
    public class AppDbContext:DbContext
    {
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-2SL889D;Database=HastaKonumDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<BedLog> BedLogs { get; set; }
        public DbSet<CleaningLog> CleaningLogs { get; set; }

    }
}
