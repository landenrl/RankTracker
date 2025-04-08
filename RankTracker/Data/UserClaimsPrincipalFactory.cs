using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RankTracker.Models;
using System.Security.Claims;
using System.Threading.Tasks;

public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);

        // Assign "User" role if the user has no roles
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0) // User has no role
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return principal;
    }
}
