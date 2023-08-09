namespace IdentityVSAPI.DAL.Initializer
{
    /// <summary>
    /// Test users initializer
    /// </summary>
    public class DbBaseUserInitializer : IDbBaseUserInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        public DbBaseUserInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<long>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(StaticUserRoles.OWNER).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole<long>(StaticUserRoles.OWNER)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole<long>(StaticUserRoles.ADMIN)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole<long>(StaticUserRoles.USER)).GetAwaiter().GetResult();
            }
            else return;

            ApplicationUser ownerUser = new ApplicationUser()
            {
                UserName = "owner@gmail.com",
                FirstName = "Anessens",
                LastName = "Anessens",
                MiddleName = "Anessens",
                Email = "owner@gmail.com",
                BirthDate = new DateTime(1991, 12, 09),
                EmailConfirmed = true,
                PasswordHash = "123456789"
            };
            _userManager.CreateAsync(ownerUser, "Admin123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(ownerUser, StaticUserRoles.OWNER).GetAwaiter().GetResult();
            var temp1 = _userManager.AddClaimsAsync(ownerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,ownerUser.FirstName+" "+ownerUser.LastName+" "+ ownerUser.MiddleName),
                new Claim(JwtClaimTypes.GivenName,ownerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,ownerUser.LastName),
                new Claim(JwtClaimTypes.MiddleName,ownerUser.MiddleName),
                new Claim(JwtClaimTypes.Role,StaticUserRoles.OWNER),
            }).Result;

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin@gmail.com",
                FirstName = "Anessens",
                LastName = "Anessens",
                MiddleName = "Anessens",
                Email = "admin@gmail.com",
                BirthDate = new DateTime(1991, 12, 09),
                EmailConfirmed = true,
                PasswordHash = "123456789"
            };
            _userManager.CreateAsync(adminUser, "Admin123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, StaticUserRoles.ADMIN).GetAwaiter().GetResult();
            var temp2 = _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+adminUser.LastName+" "+ adminUser.MiddleName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.MiddleName,adminUser.MiddleName),
                new Claim(JwtClaimTypes.Role,StaticUserRoles.ADMIN),
            }).Result;

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "user@gmail.com",
                FirstName = "Anessens",
                LastName = "Anessens",
                MiddleName = "Anessens",
                Email = "user@gmail.com",
                BirthDate = new DateTime(1991, 12, 09),
                EmailConfirmed = true,
                PasswordHash = "123456789"
            };
            _userManager.CreateAsync(user, "Admin123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, StaticUserRoles.USER).GetAwaiter().GetResult();
            var temp3 = _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,user.FirstName+" "+user.LastName+" "+ user.MiddleName),
                new Claim(JwtClaimTypes.GivenName,user.FirstName),
                new Claim(JwtClaimTypes.FamilyName,user.LastName),
                new Claim(JwtClaimTypes.MiddleName,user.MiddleName),
                new Claim(JwtClaimTypes.Role,StaticUserRoles.USER),
            }).Result;
        }
    }
}
