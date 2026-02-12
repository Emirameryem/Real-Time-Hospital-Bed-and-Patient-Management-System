using Microsoft.AspNetCore.Mvc;

namespace HastaKonum2.WebUI.Controllers
{
    [Route("Auth/[action]")]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            if (username == "admin" && password == "123" && role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (username == "hemsire" && password == "123" && role == "Hemsire")
            {
                return RedirectToAction("Index", "Hemsire");
            }
            else if (username == "temizlikci" && password == "123" && role == "Temizlikci")
            {
                return RedirectToAction("Index", "Temizlikci");
            }

            ViewBag.Hata = "Bilgiler yanlış.";
            return View();
        }


    }
}
