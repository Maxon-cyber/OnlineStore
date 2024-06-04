using OnlineStore.Core.Abstractions;
using OnlineStore.Entities.User;
using OnlineStore.MVP.Presenters.Abstractions.Contracts;
using OnlineStore.MVP.ViewModels.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.UserIdentification;
using OnlineStore.Services.EntityData.SqlServer;
using OnlineStore.Services.EntityData.SqlServer.DataProcessing;
using System.Text;

namespace OnlineStore.MVP.Presenters.UserIdentification;

public sealed class RegistrationPresenter : Presenter<IRegistrationView>
{
    private readonly UserService _userService;

    public RegistrationPresenter(IApplicationController controller, IRegistrationView view, SqlServerService service)
        : base(controller, view)
    {
        _userService = service.User;

        View.Registration += RegistrationAsync;
        View.ReturnToAuthorization += ReturnToAuthorization;
    }

    private async Task RegistrationAsync(RegistrationViewModel model)
    {
        bool isAdded = await _userService.ChangeUserAsync(TypeOfCommand.Insert, new UserEntity()
        {
            Name = model.Name,
            SecondName = model.SecondName,
            Patronymic = model.Patronymic,
            Gender = Enum.Parse<Gender>(model.Gender),
            Age = Convert.ToUInt32(model.Age),
            Location = new Location()
            {
                HouseNumber = model.HouseNumber,
                Street = model.Street,
                City = model.City,
                Region = model.Region,
                Country = model.Country
            },
            Login = model.Login,
            Password = Encoding.UTF8.GetBytes(model.Password),
            Role = UserParameters.DEFAULT_ROLE
        });

        if (isAdded)
            View.ShowMessage("Пользователь с таким логином уже зарегистрирован", "Предупреждение", MessageLevel.Warning);
        else
        {
            View.ShowMessage("Вы успешно зарегистрированы", "Успех", MessageLevel.Info);
            View.Close();
        }
    }

    private void ReturnToAuthorization()
        => View.Close();
}