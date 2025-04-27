using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RankTracker.Models
{
    /// <summary>
    /// Represents a game that users can track ranks for.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Unique identifier for the game.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the game.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Foreign key referencing the user who created the game.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// Navigation property for the user who created the game.
        /// </summary>
        public ApplicationUser? User { get; set; }
        /// <summary>
        /// Collection of rank entries associated with the game.
        /// </summary>
        public ICollection<RankEntry>? RankEntries { get; set; }
    }

}
