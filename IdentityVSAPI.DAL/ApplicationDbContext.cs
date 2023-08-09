namespace IdentityVSAPI.DAL
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            //Database.Migrate();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);  
        }
    }
}
