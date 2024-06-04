namespace OnlineStore.MVP.Presenters.Abstractions.Interfaces;

public interface IPresenter
{
    void Run();

    void Run(Action action);
}