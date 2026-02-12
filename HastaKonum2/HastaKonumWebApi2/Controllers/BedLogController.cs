using HastaKonumWebApi2.Data;
using HastaKonumWebApi2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HastaKonumWebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BedLogController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BedLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BedLog>>> GetBedLogs()
        {
            return await _context.BedLogs
                .Include(b => b.Bed)
                .Include(b => b.User)
                .ToListAsync();
        }

        // GET: api/BedLog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BedLog>> GetBedLog(int id)
        {
            var bedLog = await _context.BedLogs
                .Include(b => b.Bed)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bedLog == null)
                return NotFound();

            return bedLog;
        }

        // POST: api/BedLog
        [HttpPost]
        public async Task<ActionResult<BedLog>> PostBedLog(BedLog bedLog)
        {
            bedLog.Timestamp = DateTime.Now;

            _context.BedLogs.Add(bedLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBedLog), new { id = bedLog.Id }, bedLog);
        }

        // PUT: api/BedLog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBedLog(int id, BedLog bedLog)
        {
            if (id != bedLog.Id)
                return BadRequest();

            _context.Entry(bedLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BedLogs.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/BedLog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBedLog(int id)
        {
            var bedLog = await _context.BedLogs.FindAsync(id);
            if (bedLog == null)
                return NotFound();

            _context.BedLogs.Remove(bedLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
