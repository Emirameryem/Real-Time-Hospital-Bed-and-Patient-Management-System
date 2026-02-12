using HastaKonum2.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace HastaKonum2.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiClient");
                var viewModel = new DashboardViewModel();

                // Yatakları ve hastaları paralel olarak çek
                var bedsTask = httpClient.GetAsync("/api/Bed");
                var patientsTask = httpClient.GetAsync("/api/Patient");

                await Task.WhenAll(bedsTask, patientsTask);

                // Yatakları al
                if (bedsTask.Result.IsSuccessStatusCode)
                {
                    var bedsJson = await bedsTask.Result.Content.ReadAsStringAsync();
                    var beds = JsonSerializer.Deserialize<List<BedViewModel>>(bedsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (beds != null)
                    {
                        viewModel.Beds = beds;
                    }
                }

                // Hastaları al
                if (patientsTask.Result.IsSuccessStatusCode)
                {
                    var patientsJson = await patientsTask.Result.Content.ReadAsStringAsync();
                    var apiPatients = JsonSerializer.Deserialize<List<ApiPatientResponse>>(patientsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (apiPatients != null)
                    {
                        viewModel.Patients = apiPatients.Select(p => new PatientViewModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            BedId = p.BedId,
                            BedNumber = p.Bed?.BedNumber ?? string.Empty
                        }).ToList();
                    }
                }

                // Hastaları yataklara eşleştir
                foreach (var bed in viewModel.Beds)
                {
                    var patient = viewModel.Patients.FirstOrDefault(p => p.BedId == bed.Id);
                    if (patient != null)
                    {
                        bed.Patient = patient;
                        patient.BedNumber = bed.BedNumber;
                    }
                }

                // İstatistikleri hesapla
                viewModel.Statistics.TotalBeds = viewModel.Beds.Count;
                viewModel.Statistics.OccupiedBeds = viewModel.Beds.Count(b => b.Status == "Dolu");
                viewModel.Statistics.EmptyBeds = viewModel.Beds.Count(b => b.Status == "Boş");
                viewModel.Statistics.CleaningBeds = viewModel.Beds.Count(b => b.Status == "Temizlikte");
                viewModel.Statistics.OccupancyRate = viewModel.Statistics.TotalBeds > 0
                    ? (double)viewModel.Statistics.OccupiedBeds / viewModel.Statistics.TotalBeds * 100
                    : 0;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard verileri yüklenirken hata oluştu");
                // Hata durumunda boş model döndür
                return View(new DashboardViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
