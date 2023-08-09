namespace IdentityVSAPI.Domain.Entity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime BirthDate { get; init; }
    }
}
