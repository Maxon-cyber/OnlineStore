using OnlineStore.Core.Abstractions;
using OnlineStore.Entities.User;
using OnlineStore.MVP.Presenters.Abstractions.Contracts;
using OnlineStore.MVP.Presenters.Common;
using OnlineStore.MVP.Presenters.MainWindow;
using OnlineStore.MVP.ViewModels.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.UserIdentification;
using OnlineStore.Services.EntityData.SqlServer;
using OnlineStore.Services.EntityData.SqlServer.DataProcessing;
using OnlineStore.Services.Utilities.Caching.InFile;
using OnlineStore.Services.Utilities.Caching.InMemory;
using System.Text;

namespace OnlineStore.MVP.Presenters.UserIdentification;

public sealed class AuthorizationPresenter : Presenter<IAuthorizationView>
{
    private readonly UserService _userService;
    private readonly FileCache<UserEntity> _fileCache;
    private readonly MemoryCache _memoryCache;


    public AuthorizationPresenter(IApplicationController controller, IAuthorizationView view, FileCache<UserEntity> fileCache, SqlServerService service)
        : base(controller, view)
    {
        _userService = service.User;
        _fileCache = fileCache.SetFile("User");
        _memoryCache = MemoryCache.Instance;

        View.Authorization += LoginAsync;
        View.Registration += Registration;
    }

    private async Task LoginAsync(AuthorizationViewModel model, bool rememberMe)
    {
        UserEntity? user = await _userService.GetUserByAsync(new UserEntity
        {
            Login = model.Login,
            Password = Encoding.UTF8.GetBytes(model.Password)
        });

        if (user == null)
        {
            View.ShowMessage("Неправильный логин или пароль", "Ошибка", MessageLevel.Error);
            return;
        }

        View.ShowMessage($"Добро Пожаловать, {user}!", "Добро пожаловать", MessageLevel.Info);

        switch (user.Role)
        {
            case Role.User:
                Controller.Run<MainWindowPresenter>();

                if (rememberMe)
                {
                    await _fileCache.WriteWithEncryptionAsync("User", user);
                    Settings.Default.RememberMe = true;
                    Settings.Default.Save();
                }
                else
                    await _memoryCache.WriteAsync("User", user);

                View.Close();
                break;
            case Role.Admin:
                await _memoryCache.WriteAsync("Admin", user);
                View.Close();
                break;
        }
    }

    private IRegistrationView Registration()
        => Controller.Run<IRegistrationView, RegistrationPresenter>();
}