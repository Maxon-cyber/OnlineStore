using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Configuration.Abstractions;
using OnlineStore.Core.IoC.Abstractions;
using OnlineStore.MVP.Presenters.Abstractions.Interfaces;

namespace OnlineStore.Core;

public sealed class ApplicationController<TIoCContainer, TApplicationConfiguration> : IApplicationController
    where TIoCContainer : IIoCBuilder, new()
    where TApplicationConfiguration : IApplicationConfigurationBuilder, new()
{
    public TIoCContainer Container { get; }

    public TApplicationConfiguration Configuration { get; }

    public ApplicationController()
        => (Container, Configuration) = (new TIoCContainer(), new TApplicationConfiguration());

    public void Run<TPresenter>()
        where TPresenter : class, IPresenter
    {
        TPresenter presenter = Container.Resolve<TPresenter>();
        presenter.Run();
    }

    public void Run<TPresenter>(Action action)
        where TPresenter : class, IPresenter
    {
        TPresenter presenter = Container.Resolve<TPresenter>();
        presenter.Run(action);
    }

    public TReturnedValue Run<TReturnedValue, TPresenter>()
        where TReturnedValue : notnull
        where TPresenter : class, IPresenter
    {
        TPresenter presenter = Container.Resolve<TPresenter>();
        TReturnedValue returnedValue = Container.Resolve<TReturnedValue>();

        presenter.Run();

        return returnedValue;
    }
}