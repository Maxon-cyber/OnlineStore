using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;

namespace OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;

public interface IUserAccountView : IView
{
    event Func<Task<UserAccountModel?>> LoadUserData;
    event Func<UserAccountModel, Task> UpdateData;
    event Func<Task> LogOutFromAccount;
}