namespace IdentityVSAPI.Service.Helpers
{
    public static class JwtBearerExtensions
    {
        /// <summary>
        /// Create claims
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns>Claims.</returns>
        public static List<Claim> CreateClaims(this ApplicationUser user, List<IdentityRole<long>> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Role, string.Join(" ", roles.Select(x => x.Name))),
            };
            return claims;
        }
        /// <summary>
        /// Create security token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="configuration"></param>
        /// <returns>Token.</returns>
        public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims, IConfiguration configuration)
        {
            return new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: configuration.CreateSigningCredentials()
            );
        }
        /// <summary>
        /// Create signing credentials
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>Signing credentials.</returns>
        public static SigningCredentials CreateSigningCredentials(this IConfiguration configuration)
        {
            return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("3uhqHXxc._]$iV55G.WG{NtG4jKQ5wL*")
            ),
            SecurityAlgorithms.HmacSha256);
        }
        /// <summary>
        /// Create refresh token
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>Token.</returns>
        public static string GenerateRefreshToken(this IConfiguration configuration)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
