namespace IdentityVSAPI.Service.Interfaces
{
    public interface IAccountServicesAuthAsync
    {
        Task<AuthResponse> RegisterAsync(RegisterDto register);
        Task<AuthResponse> AuthenticateAsync(AuthenticationDto authen);
        Task<AuthResponse> MakeAdminAsync(UpdateUserPermissionDto updateUser);
        Task<AuthResponse> MakeOwnerAsync(UpdateUserPermissionDto updateUser);
    }
}
