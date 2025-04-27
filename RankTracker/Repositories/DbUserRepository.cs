using Microsoft.EntityFrameworkCore;
using RankTracker.Data;
using RankTracker.Models;

namespace RankTracker.Repositories
{
    /// <summary>
    /// Repository class for accessing ApplicationUser data from the database.
    /// Implements methods for querying user information.
    /// </summary>
    public class DbUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes the DbUserRepository with the database context.
        /// </summary>
        /// <param name="db">Application database context.</param>
        public DbUserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        /// <summary>
        /// Retrieves an ApplicationUser based on their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The matching ApplicationUser if found; otherwise, null.</returns>
        public async Task<ApplicationUser?> ReadByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}
