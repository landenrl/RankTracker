using RankTracker.Models;

namespace RankTracker.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> ReadByUsernameAsync(string username);
    }
}
