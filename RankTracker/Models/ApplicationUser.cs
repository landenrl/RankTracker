using Microsoft.AspNetCore.Identity;

namespace RankTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RankEntry> RankEntries { get; set; } = new List<RankEntry>();
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
