namespace IdentityVSAPI.Service.Helpers
{
    public class Token
    {
        private readonly IConfiguration _configuration;

        public Token(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Create token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns>Token</returns>
        public string CreateToken(ApplicationUser user, List<IdentityRole<long>> roles)
        {
            var token = user
            .CreateClaims(roles)
            .CreateJwtToken(_configuration);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}

