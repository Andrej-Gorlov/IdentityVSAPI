namespace IdentityVSAPI.DAL.Interfaces
{
    public interface IIdentityRepository
    {
        Task<IdentityResult> CreateAsync(ApplicationUser newUser, string password);
        Task<ApplicationUser?> FindByAsync(string searchProperty);
        Task<ApplicationUser?> GetByAsync(Expression<Func<ApplicationUser, bool>> filter, bool tracking = true);
        Task<List<IdentityRole<long>>> GetRolesAsync(long userId);
        Task AddToRoleAsync(ApplicationUser newUser, string role);
        Task SeveAsync();
        Task<bool> ConfirmPasswordAsync(ApplicationUser user, string password);
    }
}
