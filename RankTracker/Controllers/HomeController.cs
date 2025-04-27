using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RankTracker.Models;
using System.Diagnostics;
using RankTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace RankTracker.Controllers
{
    /// <summary>
    /// Controller for handling home page, dashboard, privacy policy, and error pages.
    /// Manages both general and user-specific dashboard data.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes the HomeController with the logger, database context, and user manager.
        /// </summary>
        /// <param name="logger">Logger for recording application events.</param>
        /// <param name="context">Application database context.</param>
        /// <param name="userManager">ASP.NET Identity user manager.</param>
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the user dashboard with total games, total rank entries, and recent user rank entries.
        /// </summary>
        /// <returns>The Dashboard view populated with aggregated data.</returns>
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);

            var totalGames = await _context.Games.CountAsync();
            var totalRankEntries = await _context.RankEntries.CountAsync();
            var userRankEntries = await _context.RankEntries
                                                .Where(re => re.UserId == userId)
                                                .OrderByDescending(re => re.Date)
                                                .Take(5)
                                                .ToListAsync();
            var dashboardData = new
            {
                TotalGames = totalGames,
                TotalRankEntries = totalRankEntries,
                UserRankEntries = userRankEntries
            };
            return View(dashboardData);
        }

        /// <summary>
        /// Displays the application's main landing page.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the application's privacy policy page.
        /// </summary>
        /// <returns>The Privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page when an unhandled exception occurs.
        /// </summary>
        /// <returns>The Error view with error details.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
