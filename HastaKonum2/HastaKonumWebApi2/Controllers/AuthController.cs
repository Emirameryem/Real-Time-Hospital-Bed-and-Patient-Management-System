using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HastaKonumWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Giriş işlemi
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            if (string.IsNullOrWhiteSpace(_configuration["Jwt:Key"]))
            {
                return BadRequest("JWT Key is missing in appsettings.json.");
            }

            // Basit kullanıcı kontrolü (gerçek projede veritabanı kullanılır)
            if (login.Username == "admin" && login.Password == "1234")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = tokenString });
            }

            return Unauthorized("Kullanıcı adı veya şifre hatalı.");
        }

        // DTO sınıfı
        public class UserLogin
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}

