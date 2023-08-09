namespace IdentityVSAPI.Domain.Entity.Dto
{
    public readonly struct UpdateUserPermissionDto
    {
        public UpdateUserPermissionDto(string mail)
        {
            Email = mail;
        }
        [Required(ErrorMessage = "Tребуется электронная почта пользователя.")]
        public string Email { get; init; }
    }
}
