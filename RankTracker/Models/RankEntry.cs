using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RankTracker.Models
{
    /// <summary>
    /// Represents a user's rank entry for a specific game at a specific time.
    /// </summary>
    public class RankEntry
    {
        /// <summary>
        /// Unique identifier for the rank entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The player's rank value. Must be between 1 and 5000.
        /// </summary>
        [Required]
        [Range(1, 5000, ErrorMessage = "Rank must be between 1 and 5000.")]
        public int Rank { get; set; }
        /// <summary>
        /// The date and time when the rank was recorded.
        /// </summary>
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Optional description for the rank entry (e.g., notes about the match).
        /// </summary>
        public string? Description {  get; set; }

        /// <summary>
        /// Foreign key referencing the user who owns this rank entry.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// Navigation property for the user associated with this rank entry.
        /// </summary>
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Foreign key referencing the game associated with this rank entry.
        /// </summary>
        public int GameId { get; set; }
        [ForeignKey("GameId")]

        /// <summary>
        /// Navigation property for the game associated with this rank entry.
        /// </summary>
        public Game? Game { get; set; }
    }
}
