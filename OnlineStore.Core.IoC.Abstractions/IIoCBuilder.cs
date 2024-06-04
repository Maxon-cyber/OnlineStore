namespace OnlineStore.Core.IoC.Abstractions;

public enum Lifetime
{
    Transient = 0,
    Singleton = 1
}

public interface IIoCBuilder
{
    IIoCBuilder Register<TService>(Lifetime lifetime = Lifetime.Transient)
        where TService : notnull;

    IIoCBuilder RegisterInstance<TInstance>(TInstance instance, Lifetime lifetime = Lifetime.Transient)
        where TInstance : class;

    IIoCBuilder RegisterView<TView, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TView : notnull
        where TImplementation : notnull, TView;

    IIoCBuilder Register<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient, bool asSelf = false)
        where TService : notnull
        where TImplementation : notnull, TService;

    IIoCBuilder RegisterGeneric(Type type, Lifetime lifetime = Lifetime.Transient);

    IIoCBuilder RegisterGenericWithConstructor(Type type, string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient);

    IIoCBuilder RegisterWithConstructor<TService>(string nameParameter, object parameter, Lifetime lifetime = Lifetime.Transient)
        where TService : notnull;

    TService Resolve<TService>()
       where TService : notnull;

    void Build();
}