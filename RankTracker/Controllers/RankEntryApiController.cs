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
    /// <summary>
    /// API controller for managing RankEntry entities.
    /// Provides endpoints to create, read, update, and delete rank entries.
    /// </summary>
    [EnableCors]
    [Route("api/rankentry")]
    [ApiController]
    public class RankEntryApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes the RankEntryApiController with the database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public RankEntryApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all rank entries, including related Game and User information.
        /// </summary>
        /// <returns>A list of all rank entries.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RankEntry>>> GetRankEntries()
        {
            return await _context.RankEntries
                .Include(r => r.Game) // Include Game details
                .Include(r => r.User) // Include User details
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific rank entry by ID.
        /// </summary>
        /// <param name="id">The ID of the rank entry to retrieve.</param>
        /// <returns>The requested rank entry or a NotFound result if not found.</returns>
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

        /// <summary>
        /// Creates a new rank entry.
        /// </summary>
        /// <param name="rankEntry">The RankEntry object to create.</param>
        /// <returns>The created rank entry with a 201 Created status, or error response.</returns>
        [HttpPost]
        public async Task<ActionResult<RankEntry>> CreateRankEntry([FromBody] RankEntry rankEntry)
        {

            var gm = await _context.Games.FindAsync(rankEntry.GameId);
            rankEntry.Game = gm;

            _context.RankEntries.Add(rankEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRankEntry), new { id = rankEntry.Id }, rankEntry);
        }

        /// <summary>
        /// Updates an existing rank entry.
        /// </summary>
        /// <param name="id">The ID of the rank entry to update.</param>
        /// <param name="rankEntry">The updated rank entry object.</param>
        /// <returns>NoContent if successful, or appropriate error status.</returns>
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

        /// <summary>
        /// Deletes a rank entry by ID.
        /// </summary>
        /// <param name="id">The ID of the rank entry to delete.</param>
        /// <returns>NoContent if deletion successful, or NotFound if entry does not exist.</returns>
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
