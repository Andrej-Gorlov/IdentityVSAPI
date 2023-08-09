namespace IdentityVSAPI.Domain.Entity.Dto
{
    public record struct RegisterDto
    {
        public RegisterDto(string email, DateTime birthDate, string password, string passwordConfirm, string firstName, string lastName, string? middleName, string role)
        {
            Email = email;
            BirthDate = birthDate;
            Password = password;
            PasswordConfirm = passwordConfirm;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Role = role;
        }

        [EmailAddress, Required(ErrorMessage = "Введите электронную почту.")]
        public string Email { get; init; }

        [Required, Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; init; }

        [Display(Name = "Пароль"), Required(ErrorMessage = "Введите пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Display(Name = "Подтвердить пароль"), Required(ErrorMessage = "Введите пароль.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; init; }

        [Display(Name = "Имя"), Required(ErrorMessage = "Введите имя пользователя.")]
        public string FirstName { get; init; }

        [Display(Name = "Фамилия"), Required(ErrorMessage = "Введите фамилию пользователя.")]
        public string LastName { get; init; }

        [Display(Name = "Отчество")]
        public string? MiddleName { get; init; }
        [Display(Name = "Роль")]
        public string? Role { get; init; }
    }
}
