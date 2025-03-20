using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankTracker.Data;
using RankTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankTracker.Controllers.Api
{
    [EnableCors]
    [Route("api/rankentry")]
    [ApiController]
    public class RankEntryApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RankEntryApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/rankentry (Get All Rank Entries)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RankEntry>>> GetRankEntries()
        {
            return await _context.RankEntries
                .Include(r => r.Game) // Include Game details
                .Include(r => r.User) // Include User details
                .ToListAsync();
        }

        // ✅ GET: api/rankentry/5 (Get a Specific Rank Entry by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<RankEntry>> GetRankEntry(int id)
        {
            var rankEntry = await _context.RankEntries
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rankEntry == null)
            {
                return NotFound();
            }

            return rankEntry;
        }

        // ✅ POST: api/rankentry (Create a Rank Entry)
        [HttpPost]
        public async Task<ActionResult<RankEntry>> CreateRankEntry([FromBody] RankEntry rankEntry)
        {

            var gm = await _context.Games.FindAsync(rankEntry.GameId);
            rankEntry.Game = gm;

            _context.RankEntries.Add(rankEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRankEntry), new { id = rankEntry.Id }, rankEntry);
        }

        // ✅ PUT: api/rankentry/5 (Update a Rank Entry)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRankEntry(int id, RankEntry rankEntry)
        {
            if (id != rankEntry.Id)
            {
                return BadRequest();
            }

            var existingRankEntry = await _context.RankEntries.FindAsync(id);
            if (existingRankEntry == null)
            {
                return NotFound();
            }

            var gm = await _context.Games.FindAsync(rankEntry.GameId);
            existingRankEntry.Game = gm;

            // Update values
            existingRankEntry.Rank = rankEntry.Rank;
            existingRankEntry.Date = rankEntry.Date;
            existingRankEntry.Description = rankEntry.Description;
            existingRankEntry.GameId = rankEntry.GameId;

            _context.Entry(existingRankEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/rankentry/5 (Delete a Rank Entry)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRankEntry(int id)
        {
            var rankEntry = await _context.RankEntries.FindAsync(id);
            if (rankEntry == null)
            {
                return NotFound();
            }

            _context.RankEntries.Remove(rankEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
