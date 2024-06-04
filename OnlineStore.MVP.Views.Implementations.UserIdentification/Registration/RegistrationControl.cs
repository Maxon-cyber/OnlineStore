using OnlineStore.MVP.ViewModels.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.UserIdentification;

namespace OnlineStore.MVP.Views.Implementations.UserIdentification.Registration;

public sealed partial class RegistrationControl : UserControl, IRegistrationView
{
    public RegistrationControl()
    {
        _components = new System.ComponentModel.Container();

        InitializeComponent();
    }

    public event Func<RegistrationViewModel, Task> Registration;
    public event Action? ReturnToAuthorization;

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void ShowMessage(string message, string caption, MessageLevel level)
    {
        throw new NotImplementedException();
    }
}