namespace IdentityVSAPI.Domain.Response
{
    public enum Status
    {
        NotFound,
        PasswordInvalid,
        Authorized,
        Unauthorized,
        Exists, // user already registered
        Mismatch,
        NotCreated,
        UpdatedRole
    }
}
