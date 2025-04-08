using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankTracker.Data;
using RankTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankTracker.Controllers
{
    [EnableCors]
    [Route("api/Game")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GameApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ GET: api/Game
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.Include(g => g.User).ToListAsync();
        }

        // ✅ GET: api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return NotFound();
            return game;
        }

        // ✅ POST: api/Game (Create Game)
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(Game game)
        {

            if (string.IsNullOrWhiteSpace(game.Name))
            {
                return BadRequest("Game name is required.");
            }

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        // ✅ PUT: api/Game/5 (Update Game)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            var existingGame = await _context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (existingGame == null) return NotFound();


            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/Game/5 (Delete Game)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();


            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

