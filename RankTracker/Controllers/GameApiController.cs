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
    /// <summary>
    /// API controller for managing Game entities.
    /// Provides endpoints to create, read, update, and delete games.
    /// </summary>
    [EnableCors]
    [Route("api/Game")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor to initialize GameApiController with database context and user manager.
        /// </summary>
        /// <param name="context">Database context for accessing game data.</param>
        /// <param name="userManager">User manager for managing ApplicationUser entities.</param>
        public GameApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves all games from the database.
        /// </summary>
        /// <returns>A list of all games, including associated users.</returns>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.Include(g => g.User).ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific game by ID.
        /// </summary>
        /// <param name="id">The ID of the game to retrieve.</param>
        /// <returns>The requested game, or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return NotFound();
            return game;
        }

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="game">The game object to create.</param>
        /// <returns>The created game with a 201 status, or a 400 error if validation fails.</returns>
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

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="id">The ID of the game to update.</param>
        /// <param name="game">The updated game object.</param>
        /// <returns>NoContent if successful, BadRequest or NotFound otherwise.</returns>
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

        /// <summary>
        /// Deletes a game by ID.
        /// </summary>
        /// <param name="id">The ID of the game to delete.</param>
        /// <returns>NoContent if successful, or NotFound if the game does not exist.</returns>
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

