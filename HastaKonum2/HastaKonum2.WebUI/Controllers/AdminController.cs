using Microsoft.AspNetCore.Mvc;

namespace HastaKonum2.WebUI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
