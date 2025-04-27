using Microsoft.AspNetCore.Identity;

namespace RankTracker.Models
{
    /// <summary>
    /// Extends the default IdentityUser with additional relationships to games and rank entries.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Collection of rank entries associated with the user.
        /// </summary>
        public ICollection<RankEntry> RankEntries { get; set; } = new List<RankEntry>();
        /// <summary>
        /// Collection of games created by the user.
        /// </summary>
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
