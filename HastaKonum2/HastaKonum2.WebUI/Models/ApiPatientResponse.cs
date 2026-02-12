namespace HastaKonum2.WebUI.Models
{
    // API'den gelen Patient response modeli (nested Bed object içerir)
    public class ApiPatientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BedId { get; set; }
        public ApiBedResponse? Bed { get; set; }
    }

    public class ApiBedResponse
    {
        public int Id { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}

