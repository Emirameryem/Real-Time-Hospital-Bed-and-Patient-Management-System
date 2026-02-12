namespace HastaKonum2.WebUI.Models
{
    public class DashboardViewModel
    {
        public List<BedViewModel> Beds { get; set; } = new();
        public List<PatientViewModel> Patients { get; set; } = new();
        public BedStatistics Statistics { get; set; } = new();
    }

    public class BedViewModel
    {
        public int Id { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public PatientViewModel? Patient { get; set; }
    }

    public class PatientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BedId { get; set; }
        public string? BedNumber { get; set; }
    }

    public class BedStatistics
    {
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int EmptyBeds { get; set; }
        public int CleaningBeds { get; set; }
        public double OccupancyRate { get; set; }
    }
}

