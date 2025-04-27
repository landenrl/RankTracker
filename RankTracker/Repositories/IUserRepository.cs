using RankTracker.Models;

namespace RankTracker.Repositories
{
    /// <summary>
    /// Interface for user repository functionality.
    /// Defines methods for reading ApplicationUser data.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves an ApplicationUser based on the provided username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The matching ApplicationUser if found; otherwise, null.</returns>
        Task<ApplicationUser?> ReadByUsernameAsync(string username);
    }
}
