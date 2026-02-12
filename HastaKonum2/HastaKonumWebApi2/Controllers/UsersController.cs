using HastaKonumWebApi2.Data;
using HastaKonumWebApi2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using HastaKonumWebApi2.Dto;

namespace HastaKonumWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm kullanıcıları getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UsersDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role
                })
                .ToListAsync();
        }

        // ID'ye göre kullanıcı getir
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        // CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUsersDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new Users
            {
                Username = dto.Username,
                Password = hashedPassword,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new UsersDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return Ok(result);
        }

        // UpdateUser
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.Id)
                return BadRequest();

            // Mevcut kullanıcıyı veritabanından çek
            var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return NotFound();

            // Eğer password boş değilse hashle, boşsa eski şifreyi koru
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            } 
            {
                user.Password = existingUser.Password;
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Kullanıcı sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
