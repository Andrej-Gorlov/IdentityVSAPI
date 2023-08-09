namespace IdentityVSAPI.Service.Implementations
{
    public class AccountServicesAuthAsync: IAccountServicesAuthAsync
    {
        private readonly IIdentityRepository _identityRep;
        private readonly ILogger<AccountServicesAuthAsync> _logger;
        private readonly IConfiguration _configuration;
        private readonly AuthResponse response;
        private readonly Token token;
        public AccountServicesAuthAsync(ILogger<AccountServicesAuthAsync> logger, IIdentityRepository identityRepository, IConfiguration configuration)
        {
            _logger = logger;
            _identityRep = identityRepository;
            _configuration = configuration;
            response = new();
            token = new(configuration);
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="register">Data of the registering user</param>
        /// <returns>Authenticated user.</returns>
        public async Task<AuthResponse> RegisterAsync(RegisterDto register)
        {
            _logger.LogInformation($"Регистрация пользовотеля. / method: Register");
            if (register.Password != register.PasswordConfirm)
            {
                _logger.LogWarning($"Пароли не совпадают.");
                response.Status = Status.Mismatch;
                response.Message = $"Пароли не совпадают.";
                return response;
            }

            var isExistsUser = await _identityRep.FindByAsync(register.Email);
            if (isExistsUser != null)
            {
                _logger.LogInformation($"Пользовотелm c email {register.Email} уже зарегистрирован.");
                response.Status = Status.Exists;
                response.Message = $"Пользовотелm c email {register.Email} уже зарегистрирован.";
                return response;
            }

            var newUser = new ApplicationUser
            {
                UserName = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                MiddleName = register.MiddleName,
                Email = register.Email,
                BirthDate = register.BirthDate,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _identityRep.CreateAsync(newUser, register.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "Не удалось создать пользователя, потому что: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                _logger.LogWarning(errorString);
                response.Status = Status.NotCreated;
                response.Message = errorString;
                return response;
            }

            var role = register.Role == null ? StaticUserRoles.USER : register.Role;
            _logger.LogWarning($"Добавление роли {role}.");
            await _identityRep.AddToRoleAsync(newUser, role);

            return await AuthenticateAsync(new() { Email = register.Email, Password = register.Password});
        }
        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="authen"></param>
        /// <returns>Authenticated user.</returns>
        public async Task<AuthResponse> AuthenticateAsync(AuthenticationDto authen)
        {
            _logger.LogInformation($"Аутентификация пользовотеля. / method: Authenticate");
            var managedUser = await _identityRep.FindByAsync(authen.Email);
            if (managedUser is null) return UserNotFound(authen.Email);

            var isPasswordValid = await _identityRep.ConfirmPasswordAsync(managedUser, authen.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Неверный пароль.");
                response.Status = Status.PasswordInvalid;
                response.Message = "Неверный пароль.";
                return response;
            }

            var user = await _identityRep.GetByAsync(u => u.Email == authen.Email);
            if (user is null)
            {
                _logger.LogWarning("Пользователь неавторизован.");
                response.Status = Status.Unauthorized;
                response.Message = "Пользователь неавторизован.";
                return response;
            }

            var roles = await _identityRep.GetRolesAsync(user.Id);

            _logger.LogWarning("Cоздание токена.");
            var accessToken = token.CreateToken(user, roles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _logger.LogWarning("Сохраненные в бд.");
            await _identityRep.SeveAsync();

            _logger.LogWarning("Пользователь авторизован.");
            response.resultAuth = new()
            {
                Username = user.UserName,
                Email = user.Email,
                Token = accessToken,
                RefreshToken = user.RefreshToken
            };
            response.Status = Status.Authorized;
            response.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
            response.Message = "Пользователь авторизован.";
            return response;
        }
        /// <summary>
        /// Updating the role (ADMIN).
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns>A user with the role (ADMIN).</returns>
        public async Task<AuthResponse> MakeAdminAsync(UpdateUserPermissionDto updateUser)
        {
            _logger.LogInformation($"Обновление роли (ADMIN). / method: MakeAdminAsync");
            var user = await _identityRep.FindByAsync(updateUser.Email);
            if (user is null) return UserNotFound(updateUser.Email);

            _logger.LogInformation($"Обновление роли (ADMIN).");
            await _identityRep.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            _logger.LogInformation($"Роль пользователь {user.UserName} обновлена на (ADMIN).");
            response.Status = Status.UpdatedRole;
            response.Message = $"Роль пользователь {user.UserName} обновлена на (ADMIN).";

            return response;
        }
        /// <summary>
        /// Updating the role (OWNER).
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns>A user with the role (OWNER).</returns>
        public async Task<AuthResponse> MakeOwnerAsync(UpdateUserPermissionDto updateUser)
        {
            _logger.LogInformation($"Обновление роли (OWNER). / method: MakeOwnerAsync");
            var user = await _identityRep.FindByAsync(updateUser.Email);
            if (user is null) return UserNotFound(updateUser.Email);

            _logger.LogInformation($"Обновление роли (OWNER).");
            await _identityRep.AddToRoleAsync(user, StaticUserRoles.OWNER);

            _logger.LogInformation($"Роль пользователь {user.UserName} обновлена на (OWNER).");
            response.Status = Status.UpdatedRole;
            response.Message = $"Роль пользователь {user.UserName} обновлена на (OWNER).";
            //response.resultAuth = new()
            //{
            //    Username = user.UserName,
            //    Email = user.Email,
            //};
            return response;
        }
        /// <summary>
        /// Message about a user not found
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Message not found user.</returns>
        private AuthResponse UserNotFound(string email)
        {
            _logger.LogWarning($"Пользователь с {email} не найден.");
            response.Status = Status.NotFound;
            response.Message = $"Пользователь с {email} не найден.";
            return response;
        }
    }
}
