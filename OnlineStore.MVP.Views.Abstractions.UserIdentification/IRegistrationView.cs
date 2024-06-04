using OnlineStore.MVP.ViewModels.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.MainInterface;

namespace OnlineStore.MVP.Views.Abstractions.UserIdentification;

public interface IRegistrationView : IView
{
    event Func<RegistrationViewModel, Task> Registration;
    event Action? ReturnToAuthorization;
}