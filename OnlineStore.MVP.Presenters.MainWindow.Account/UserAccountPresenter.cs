using OnlineStore.Core.Abstractions;
using OnlineStore.Entities.User;
using OnlineStore.MVP.Presenters.Abstractions.Contracts;
using OnlineStore.MVP.Presenters.Common;
using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;
using OnlineStore.Services.EntityData.SqlServer;
using OnlineStore.Services.EntityData.SqlServer.DataProcessing;
using OnlineStore.Services.Utilities.Caching.InFile;
using OnlineStore.Services.Utilities.Caching.InMemory;
using System.Text;

namespace OnlineStore.MVP.Presenters.MainWindow.Account;

public sealed class UserAccountPresenter : Presenter<IUserAccountView>
{
    private readonly UserService _service;
    private readonly MemoryCache _memoryCache;
    private readonly FileCache<UserEntity> _fileCache;

    public UserAccountPresenter(IApplicationController controller, IUserAccountView view, FileCache<UserEntity> fileCache, SqlServerService service)
        : base(controller, view)
    {
        _memoryCache = MemoryCache.Instance;
        _fileCache = fileCache.SetFile("User");

        _service = service.User;

        View.LoadUserData += LoadUserDataAsync;
        View.UpdateData += UpdateDataAsync;
        View.LogOutFromAccount += LogOutFromAccountAsync;
    }

    private async Task<UserAccountModel?> LoadUserDataAsync()
    {
        UserEntity? user = (UserEntity)await _memoryCache.ReadByKeyAsync("User");

        if (user == null)
        {
            user = await _fileCache.ReadEnctryptedByKeyAsync("User");
            if (user == null)
            {
                View.ShowMessage("Не удалось загрузить данные", "Ой...", MessageLevel.Error);
                return await Task.FromResult<UserAccountModel?>(null);
            }
        }

        UserAccountModel userAccount = new UserAccountModel()
        {
            Name = user.Name,
            SecondName = user.SecondName,
            Patronymic = user.Patronymic,
            Gender = user.Gender.ToString(),
            Age = (int)user.Age,
            Login = user.Login,
            Password = Encoding.UTF8.GetString(user.Password),
            HouseNumber = user.Location.HouseNumber,
            Street = user.Location.Street,
            City = user.Location.City,
            Region = user.Location.Region,
            Country = user.Location.Country
        };

        return userAccount;
    }

    private async Task UpdateDataAsync(UserAccountModel model)
    {
        bool result = await _service.ChangeUserAsync(TypeOfCommand.Insert, new UserEntity()
        {
            Name = model.Name,
            SecondName = model.SecondName,
            Patronymic = model.Patronymic,
            Gender = Enum.Parse<Gender>(model.Gender),
            Age = Convert.ToUInt32(model.Age),
            Login = model.Login,
            Password = Encoding.UTF8.GetBytes(model.Password),
            Location = new Location()
            {
                HouseNumber = model.HouseNumber,
                Street = model.Street,
                City = model.City,
                Region = model.Region,
                Country = model.Country
            },
        });

        if (result)
            View.ShowMessage("Не удалось обновить данные", "Ой...", MessageLevel.Error);
        else
            View.ShowMessage("Данные успешно обновлены", "Успех", MessageLevel.Info);
    }

    private async Task LogOutFromAccountAsync()
    {
        Settings.Default.RememberMe = false;
        Settings.Default.SingIn = false;
        Settings.Default.Save();

        await _fileCache.ClearAsync();
        View.Close();
    }
}