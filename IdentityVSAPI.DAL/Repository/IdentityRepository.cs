namespace IdentityVSAPI.DAL.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentityRepository> _logger;

        public IdentityRepository(ILogger<IdentityRepository> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }
        /// <summary>
        /// create role user 
        /// </summary>
        /// <param name="newUser">user</param>
        /// <param name="role"></param>
        public async Task AddToRoleAsync(ApplicationUser newUser, string role) =>
            await _userManager.AddToRoleAsync(newUser, role);
        /// <summary>
        /// Password comparison.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>Сomparison result.</returns>
        public async Task<bool> ConfirmPasswordAsync(ApplicationUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);
        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="password"></param>
        /// <returns>Identity resul.t</returns>
        public async Task<IdentityResult> CreateAsync(ApplicationUser newUser, string password) =>
            await _userManager.CreateAsync(newUser, password);
        /// <summary>
        /// Search user by property.
        /// </summary>
        /// <param name="searchProperty"></param>
        /// <returns>User.</returns>
        public async Task<ApplicationUser?> FindByAsync(string searchProperty) =>
            await _userManager.FindByNameAsync(searchProperty);
        /// <summary>
        /// Search user by filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="tracking"></param>
        /// <returns>User.</returns>
        public async Task<ApplicationUser?> GetByAsync(Expression<Func<ApplicationUser, bool>> filter, bool tracking = true)
        {
            IQueryable<ApplicationUser> users = _db.Users;
            if (!tracking)
            {
                _logger.LogInformation($"Применен AsNoTracking. Данные не помещены в кэш");
                users = users.AsNoTracking();
            }
            _logger.LogInformation($"Возвращен отфильтрованный список. Filter: {filter.Body},Type: {filter.Type}.");
            return await users.FirstOrDefaultAsync(filter);
        }
        /// <summary>
        /// Extraction roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Roles user.</returns>
        public async Task<List<IdentityRole<long>>> GetRolesAsync(long userId)
        {
            _logger.LogInformation($"Извлечение id ролей из БД пользовотеля под id: {userId}");
            var roleIds = await _db.UserRoles.Where(r => r.UserId == userId).Select(x => x.RoleId).ToListAsync();
            _logger.LogInformation($"Извлечение ролей пользовотеля из БД под id: {string.Join(", ", roleIds)}");
            var roles = _db.Roles.Where(x => roleIds.Contains(x.Id)).ToList();
            return roles;
        }
        /// <summary>
        /// Save in BD
        /// </summary>
        public async Task SeveAsync() =>
             await _db.SaveChangesAsync();
    }
}
