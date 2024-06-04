using OnlineStore.Core.Abstractions;
using OnlineStore.MVP.Presenters.Abstractions.Interfaces;
using OnlineStore.MVP.Views.Abstractions.MainInterface;

namespace OnlineStore.MVP.Presenters.Abstractions.Contracts;

public abstract class Presenter<TView> : IPresenter
    where TView : IView
{
    protected TView View { get; }

    protected IApplicationController Controller { get; }

    protected Presenter(IApplicationController controller, TView view)
        => (Controller, View) = (controller, view);

    public void Run()
        => View.Show();

    public void Run(Action action)
    {
        action();
        View.Show();
    }
}