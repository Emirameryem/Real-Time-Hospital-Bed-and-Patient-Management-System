using HastaKonumWebApi2.Data;
using HastaKonumWebApi2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HastaKonumWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CleaningLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CleaningLogController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm temizlik loglarını getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CleaningLog>>> GetCleaningLogs()
        {
            return await _context.CleaningLogs.Include(c => c.Bed).Include(c => c.Cleaner).ToListAsync();
        }

        // ID'ye göre log getir
        [HttpGet("{id}")]
        public async Task<ActionResult<CleaningLog>> GetCleaningLog(int id)
        {
            var log = await _context.CleaningLogs.Include(c => c.Bed).Include(c => c.Cleaner).FirstOrDefaultAsync(c => c.Id == id);

            if (log == null)
                return NotFound();

            return log;
        }

        // Yeni temizlik logu oluştur
        [HttpPost]
        public async Task<ActionResult<CleaningLog>> CreateCleaningLog(CleaningLog log)
        {
            _context.CleaningLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCleaningLog), new { id = log.Id }, log);
        }

        // Temizlik logu güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCleaningLog(int id, CleaningLog log)
        {
            if (id != log.Id)
                return BadRequest();

            _context.Entry(log).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CleaningLogs.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Temizlik logu sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCleaningLog(int id)
        {
            var log = await _context.CleaningLogs.FindAsync(id);
            if (log == null)
                return NotFound();

            _context.CleaningLogs.Remove(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
