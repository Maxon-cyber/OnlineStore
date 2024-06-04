using OnlineStore.MVP.ViewModels.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.MainInterface;

namespace OnlineStore.MVP.Views.Abstractions.UserIdentification;

public interface IAuthorizationView : IView
{
    event Func<AuthorizationViewModel, bool, Task> Authorization;
    event Func<IRegistrationView> Registration;
}