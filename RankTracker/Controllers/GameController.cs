using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankTracker.Data;
using RankTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GameRankTracker.Controllers
{
    /// <summary>
    /// MVC controller for handling the Game views and CRUD operations.
    /// Allows users (Admin and User roles) to create, edit, and delete games.
    /// </summary>
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes the GameController with the database context and user manager.
        /// </summary>
        /// <param name="context">Application database context.</param>
        /// <param name="userManager">ASP.NET Identity user manager.</param>
        public GameController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays a list of all games.
        /// </summary>
        /// <returns>The Index view with the list of games.</returns>
        public async Task<IActionResult> Index()
        {
            var games = _context.Games
            .Include(r => r.User);
            return View(await games.ToListAsync());
        }

        /// <summary>
        /// Displays details for a specific game.
        /// </summary>
        /// <param name="id">ID of the game to display.</param>
        /// <returns>The Details view for the game or NotFound if the game does not exist.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);

            if (game == null) return NotFound();

            return View(game);
        }


        /// <summary>
        /// Displays the form to create a new game.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request to create a new game.
        /// </summary>
        /// <param name="game">Game object containing the new game's information.</param>
        /// <returns>Redirects to Index if successful; otherwise returns the Create view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("Name")] Game game)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            game.UserId = loggedInUser.Id;

            if (game.Name != null && game.UserId != null)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        /// <summary>
        /// Displays the form to edit an existing game.
        /// </summary>
        /// <param name="id">ID of the game to edit.</param>
        /// <returns>The Edit view for the game, or NotFound/Forbidden if not authorized.</returns>
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            var loggedInUserId = _userManager.GetUserId(User);
            if (game.UserId != loggedInUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(game);
        }

        /// <summary>
        /// Handles the POST request to edit an existing game.
        /// </summary>
        /// <param name="id">ID of the game being edited.</param>
        /// <param name="game">Game object containing updated information.</param>
        /// <returns>Redirects to Index if successful; otherwise returns the Edit view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Game game)
        {
            if (id != game.Id) return NotFound();

            var existingGame = await _context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (existingGame == null) return NotFound();

            if (existingGame.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            game.UserId = existingGame.UserId;

            if (game.Name != null && game.UserId != null)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Games.Any(e => e.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        /// <summary>
        /// Displays the form to confirm deletion of a game.
        /// </summary>
        /// <param name="id">ID of the game to delete.</param>
        /// <returns>The Delete view for the game, or NotFound/Forbidden if unauthorized.</returns>
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);
            if (game == null) return NotFound();

            var loggedInUserId = _userManager.GetUserId(User);
            if (game.UserId != loggedInUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(game);
        }

        /// <summary>
        /// Handles the POST request to confirm and delete a game.
        /// </summary>
        /// <param name="id">ID of the game to delete.</param>
        /// <returns>Redirects to Index after deletion.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                var loggedInUserId = _userManager.GetUserId(User);
                if (game.UserId != loggedInUserId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Handles AJAX requests to create a new game asynchronously.
        /// </summary>
        /// <param name="game">Game object from the AJAX request body.</param>
        /// <returns>JSON response confirming success or returning validation errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateAjax([FromBody] Game game)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            game.UserId = loggedInUser.Id;

            if (string.IsNullOrWhiteSpace(game.Name))
            {
                return BadRequest("Game name is required.");
            }

            _context.Add(game);
            await _context.SaveChangesAsync();

            return Json(new { message = "Game created successfully!" });
        }


    }
}
