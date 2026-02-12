using HastaKonumWebApi2.Data;
using HastaKonumWebApi2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HastaKonumWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BedController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Bed Tüm yatakları listeleme
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bed>>> GetBeds()
        {
            return await _context.Beds.ToListAsync();
        }

        // GET: api/Bed/5 Belirli bir yatağı ID ile getirme
        [HttpGet("{id}")]
        public async Task<ActionResult<Bed>> GetBed(int id)
        {
            var bed = await _context.Beds.FindAsync(id);

            if (bed == null)
            {
                return NotFound();
            }

            return bed;
        }

        // POST: api/Bed Yeni yatak ekleme
        [HttpPost]
        public async Task<ActionResult<Bed>> CreateBed(Bed bed)
        {
            _context.Beds.Add(bed);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBed), new { id = bed.Id }, bed);
        }

        // PUT: api/Bed/5 Yatak bilgilerini güncelleme
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateBed(int id, Bed bed)
        {
            if (id != bed.Id)
            {
                return BadRequest();
            }

            _context.Entry(bed).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Beds.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Bed/5 Yatak silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBed(int id)
        {
            var bed = await _context.Beds.FindAsync(id);
            if (bed == null)
            {
                return NotFound();
            }

            _context.Beds.Remove(bed);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

