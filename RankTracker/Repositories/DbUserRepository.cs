using Microsoft.EntityFrameworkCore;
using RankTracker.Data;
using RankTracker.Models;

namespace RankTracker.Repositories
{
    public class DbUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public DbUserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ApplicationUser?> ReadByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}
