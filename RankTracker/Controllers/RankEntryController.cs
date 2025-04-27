using RankTracker.Data;
using RankTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GameRankTracker.Controllers
{
    /// <summary>
    /// MVC controller for managing RankEntry views and operations.
    /// Provides functionality for creating, editing, viewing, and deleting rank entries.
    /// </summary>
    [Authorize]
    public class RankEntryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes the RankEntryController with the database context and user manager.
        /// </summary>
        /// <param name="context">Application database context.</param>
        /// <param name="userManager">ASP.NET Identity user manager.</param>
        public RankEntryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays a list of all rank entries.
        /// </summary>
        /// <returns>The Index view with rank entries.</returns>
        public async Task<IActionResult> Index()
        {
            var rankEntries = _context.RankEntries.Include(r => r.Game).Include(r => r.User);
            return View(await rankEntries.ToListAsync());
        }

        /// <summary>
        /// Displays details for a specific rank entry.
        /// </summary>
        /// <param name="id">The ID of the rank entry to view.</param>
        /// <returns>The Details view or NotFound if not found.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var rankEntry = await _context.RankEntries
                .Include(r => r.Game)  // Include related Game data
                .Include(r => r.User)  // Include User info if needed
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rankEntry == null) return NotFound();

            return View(rankEntry);
        }

        /// <summary>
        /// Displays the form to create a new rank entry.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Handles the POST request to create a new rank entry.
        /// </summary>
        /// <param name="rankEntry">The rank entry to create.</param>
        /// <returns>Redirects to Index if successful, otherwise redisplays form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> Create([Bind("Rank,Date,Description,GameId")] RankEntry rankEntry)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            rankEntry.UserId = loggedInUser.Id;

            var gm = await _context.Games.FindAsync(rankEntry.GameId);
            rankEntry.Game = gm;

            if (rankEntry.GameId == 0)
            {
                ModelState.AddModelError("GameId", "You must select a game.");
            }

            if (rankEntry.UserId != null && rankEntry.GameId != 0 && rankEntry.Date != DateTime.MinValue && rankEntry.Rank != null)
            {
                _context.Add(rankEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", rankEntry.GameId);
            return View(rankEntry);
        }

        /// <summary>
        /// Displays the form to edit an existing rank entry.
        /// </summary>
        /// <param name="id">The ID of the rank entry to edit.</param>
        /// <returns>The Edit view or NotFound/Forbidden.</returns>
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var rankEntry = await _context.RankEntries.FindAsync(id);
            if (rankEntry == null) return NotFound();

            var loggedInUserId = _userManager.GetUserId(User); // Get logged-in user
            if (rankEntry == null || (rankEntry.UserId != loggedInUserId && !User.IsInRole("Admin")))
            {
                return Forbid(); // Block users from editing other users' ranks
            }


            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", rankEntry.GameId);
            return View(rankEntry);
        }

        /// <summary>
        /// Handles the POST request to update an existing rank entry.
        /// </summary>
        /// <param name="id">The ID of the rank entry.</param>
        /// <param name="rankEntry">The updated rank entry object.</param>
        /// <returns>Redirects to Index if successful, otherwise redisplays form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles= "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rank,Date,Description,GameId")] RankEntry rankEntry)
        {
            if (id != rankEntry.Id) return NotFound();

            var existingRankEntry = await _context.RankEntries.AsNoTracking().FirstOrDefaultAsync(re => re.Id == id);
            if (existingRankEntry == null) return NotFound();

            var loggedInUserId = _userManager.GetUserId(User);

            if (existingRankEntry.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            rankEntry.UserId = existingRankEntry.UserId;

            if (rankEntry.UserId != null && rankEntry.GameId != 0 && rankEntry.Date != DateTime.MinValue && rankEntry.Rank != null)
            {
                try
                {
                    _context.Update(rankEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RankEntries.Any(e => e.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", rankEntry.GameId);
            return View(rankEntry);
        }

        /// <summary>
        /// Displays the confirmation page to delete a rank entry.
        /// </summary>
        /// <param name="id">The ID of the rank entry to delete.</param>
        /// <returns>The Delete view or NotFound/Forbidden.</returns>
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var rankEntry = await _context.RankEntries.Include(r => r.Game).FirstOrDefaultAsync(m => m.Id == id);
            if (rankEntry == null) return NotFound();

            var loggedInUserId = _userManager.GetUserId(User);
            if (rankEntry.UserId != loggedInUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(rankEntry);
        }

        /// <summary>
        /// Handles the POST request to delete a rank entry after confirmation.
        /// </summary>
        /// <param name="id">The ID of the rank entry to delete.</param>
        /// <returns>Redirects to Index after deletion.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rankEntry = await _context.RankEntries.FindAsync(id);
            if (rankEntry != null)
            {
                _context.RankEntries.Remove(rankEntry);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the Rank Progression page.
        /// </summary>
        /// <returns>The RankProgression view with games and users data.</returns>
        public async Task<IActionResult> RankProgression()
        {
            ViewData["Games"] = await _context.Games.ToListAsync();
            ViewData["Users"] = await _context.Users.ToListAsync();
            return View();
        }

        /// <summary>
        /// Returns rank progression data as JSON for a specific user and game.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="gameId">The ID of the game.</param>
        /// <returns>JSON list of rank progression entries.</returns>
        [HttpGet]
        public async Task<JsonResult> GetRankProgression(string userId, int gameId)
        {
            var rankEntries = await _context.RankEntries
                .Where(r => r.UserId == userId && r.GameId == gameId)
                .OrderBy(r => r.Date)
                .Select(r => new {r.Date, r.Rank})
                .ToListAsync();

            return Json(rankEntries);
        }

        /// <summary>
        /// Handles AJAX requests to create a new rank entry asynchronously.
        /// </summary>
        /// <param name="rankEntry">The rank entry object from the request body.</param>
        /// <returns>JSON response confirming success or error.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromBody] RankEntry rankEntry)
        {
            if (rankEntry == null || rankEntry.GameId == 0)
            {
                return BadRequest("Invalid data. Game must be selected.");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            rankEntry.UserId = loggedInUser.Id;

            _context.RankEntries.Add(rankEntry);
            await _context.SaveChangesAsync();

            return Json(new { message = "Rank entry created successfully!" });
        }

    }
}

