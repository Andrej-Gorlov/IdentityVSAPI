using IdentityVSAPI.DAL;
using IdentityVSAPI.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace IdentityVSAPI.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<long>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders(); ;
        }
    }
}
