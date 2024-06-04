using System.ComponentModel.DataAnnotations;

namespace OnlineStore.MVP.ViewModels.UserIdentification;

public sealed class RegistrationViewModel()
{
    [StringLength(15, ErrorMessage = "Длина имени должна быть не более 15 символов")]
    public required string Name { get; init; }

    [StringLength(20, ErrorMessage = "Длина фамилии должна быть не более 20 символов")]
    public required string SecondName { get; init; }

    [StringLength(20, ErrorMessage = "Длина отчества должна быть не более 20 символов")]
    public required string Patronymic { get; init; }

    [RegularExpression(@"^(Man|WoMan)$", ErrorMessage = "Пол должен быть либо 'Man', либо 'WoMan'")]
    public required string Gender { get; init; }

    [Range(14, 100, ErrorMessage = "Возраст может быть от 14 до 100")]
    public required int Age { get; init; }

    [StringLength(7, ErrorMessage = "Длина номер дома должна быть не более 7 символов")]
    [RegularExpression("^\\d+([/]\\d+)*([а-яА-Яa-zA-Z])?$", ErrorMessage = "Не верный формат. Возможный формат: 10, 10/9, 10б и т.д.")]
    public required string HouseNumber { get; init; }

    [StringLength(40, ErrorMessage = "Длина отчества должна быть не более 40 символов")]
    public required string Street { get; init; }

    [StringLength(25, ErrorMessage = "Длина улицы должна быть не более 25 символов")]
    public required string City { get; init; }

    [StringLength(60, ErrorMessage = "Длина отчества должна быть не более 60 символов")]
    public required string Region { get; init; }

    [StringLength(20, ErrorMessage = "Длина отчества должна быть не более 20 символов")]
    public required string Country { get; init; }

    [StringLength(15, MinimumLength = 8, ErrorMessage = "Длина логина должна быть не менее 8 и не более 15 символов")]
    [RegularExpression("^(?=(?:.*[A-Z]){2,2})(?=(?:.*\\d){2,2})(?=(?:.*[!@#$%^&*()_+\\-={}\\[\\]:;\"'<,>.?/]){2,3})[A-Za-z\\d!@#$%^&*()_+\\-={}\\[\\]:;\"'<,>.?/]{8,15}$", ErrorMessage = "Логин не соответсвует требованиям")]
    public required string Login { get; init; }

    [StringLength(20, MinimumLength = 10, ErrorMessage = "Длина логина должна быть не менее 10 и не более 20 символов")]
    [RegularExpression("^(?=(?:.*[A-Z]){3,5})(?=(?:.*\\d){3,4})(?=(?:.*[!@#$%^&*()_+\\-={}\\[\\]:;\"'<,>.?/]){2,4})[A-Za-z\\d!@#$%^&*()_+\\-={}\\[\\]:;\"'<,>.?/]{10,20}$", ErrorMessage = "Пароль не соответсвует требованиям")]
    public required string Password { get; init; }
}