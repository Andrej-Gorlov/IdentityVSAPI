namespace IdentityVSAPI.Domain.Response
{
    public class AuthResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public ResultAuthorization? resultAuth { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        //public string? Result { get; set; }
    }
}
