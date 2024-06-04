using System.ComponentModel.DataAnnotations;

namespace OnlineStore.MVP.ViewModels.UserIdentification;

public sealed class AuthorizationViewModel()
{
    [StringLength(15, MinimumLength = 8, ErrorMessage = "Длина логина должна быть не менее 8 и не более 15 символов")]
    public required string Login { get; init; }

    [StringLength(20, MinimumLength = 10, ErrorMessage = "Длина пароля должна быть не менее 10 и не более 20 символов")]
    public required string Password { get; init; }
}