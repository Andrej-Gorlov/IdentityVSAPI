namespace IdentityVSAPI.Domain.Entity.Dto
{
    public record struct AuthenticationDto
    {
        public AuthenticationDto(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
