using OnlineStore.MVP.Presenters.Abstractions.Interfaces;

namespace OnlineStore.Core.Abstractions;

public interface IApplicationController
{
    void Run<TPresenter>()
        where TPresenter : class, IPresenter;

    TReturnedValue Run<TReturnedValue, TPresenter>()
        where TReturnedValue : notnull
        where TPresenter : class, IPresenter;

    void Run<TPresenter>(Action action)
        where TPresenter : class, IPresenter;
}