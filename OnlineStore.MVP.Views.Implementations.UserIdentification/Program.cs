using Microsoft.Extensions.Configuration;
using OnlineStore.Core;
using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Configuration.Microsoft;
using OnlineStore.Core.IoC.Abstractions;
using OnlineStore.Core.IoC.Autofac;
using OnlineStore.DataAccess.Relational.Models;
using OnlineStore.DataAccess.Repositories.SqlServer;
using OnlineStore.MVP.Presenters.Common;
using OnlineStore.MVP.Presenters.MainWindow;
using OnlineStore.MVP.Presenters.MainWindow.Account;
using OnlineStore.MVP.Presenters.MainWindow.ProductShowcase;
using OnlineStore.MVP.Presenters.MainWindow.ShoppingCart;
using OnlineStore.MVP.Presenters.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.Mainwindow;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;
using OnlineStore.MVP.Views.Abstractions.UserIdentification;
using OnlineStore.MVP.Views.Implementations.MainWindow;
using OnlineStore.MVP.Views.Implementations.MainWindow.Sections;
using OnlineStore.MVP.Views.Implementations.UserIdentification.Authorization;
using OnlineStore.MVP.Views.Implementations.UserIdentification.Registration;
using OnlineStore.Services.EntityData.SqlServer;
using OnlineStore.Services.Utilities.Caching.InFile;
using OnlineStore.Services.Utilities.Logger.InFile;

namespace OnlineStore.MVP.Views.Implementations.UserIdentification;

internal static class Program
{
    private static readonly ApplicationContext _context = new ApplicationContext();

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.SetCompatibleTextRenderingDefault(false);

        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        ApplicationController<AutofacBuilder, MicrosoftConfigurationBuilder> applicationController = new ApplicationController<AutofacBuilder, MicrosoftConfigurationBuilder>();

        applicationController.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                                           .AddFile("config.yml", true, false)
                                           .AddEnviromentVariables()
                                           .Build();

        IConfigurationSection sqlServerSection = applicationController.Configuration.Root.GetSection("Databases:SqlServer");
        IDictionary<string, ConnectionParameters> parametersOfAllDatabases = new Dictionary<string, ConnectionParameters>
        {
            {
                sqlServerSection.Key,
                new ConnectionParameters()
                {
                    Provider = sqlServerSection["Key"]!,
                    Server = sqlServerSection["Server"]!,
                    Port = Convert.ToInt32(sqlServerSection["Port"]),
                    Database = sqlServerSection["Database"]!,
                    IntegratedSecurity = Convert.ToBoolean(sqlServerSection["IntegratedSecurity"]),
                    Username = sqlServerSection["Username"],
                    Password = sqlServerSection["Password"],
                    TrustedConnection = Convert.ToBoolean(sqlServerSection["TrustedConnection"]),
                    TrustServerCertificate = Convert.ToBoolean(sqlServerSection["TrustServerCertificate"]),
                    ConnectionTimeout = TimeSpan.Parse(sqlServerSection["ConnectionTimeout"]!),
                    MaxPoolSize = Convert.ToInt32(sqlServerSection["MaxPoolSize"])
                }
            },
        };

        applicationController.Container.Register<ShoppingCartPresenter>()
                                       .Register<ProductShowcasePresenter>()
                                       .Register<UserAccountPresenter>()
                                       .Register<MainWindowPresenter>()
                                       .Register<RegistrationPresenter>()
                                       .Register<AuthorizationPresenter>()
                                       .RegisterView<IMainWindowView, MainWindowForm>(Lifetime.Singleton)
                                       .RegisterView<IUserAccountView, AccountControl>(Lifetime.Singleton)
                                       .RegisterView<IProductShowcaseView, ProductShowcaseControl>(Lifetime.Singleton)
                                       .RegisterView<IShoppingCartView, ShoppingCartControl>(Lifetime.Singleton)
                                       .RegisterView<IRegistrationView, RegistrationControl>(Lifetime.Singleton)
                                       .RegisterView<IAuthorizationView, AuthorizationForm>(Lifetime.Singleton)
                                       .RegisterWithConstructor<SqlServerRepository>("connectionParameters", parametersOfAllDatabases["SqlServer"])
                                       .RegisterWithConstructor<FileLogger>("path", applicationController.Configuration.Root.GetSection("Logging:Path").Value!, Lifetime.Singleton)
                                       .RegisterGenericWithConstructor(typeof(FileCache<>), "path", applicationController.Configuration.Root.GetSection("Caching:Path").Value!, Lifetime.Singleton)
                                       .Register<SqlServerService>(Lifetime.Singleton)
                                       .RegisterInstance(_context, Lifetime.Singleton)
                                       .RegisterInstance<IApplicationController>(applicationController)
                                       .Build();

        if (Settings.Default.RememberMe)
            applicationController.Run<MainWindowPresenter>();
        else
            applicationController.Run<AuthorizationPresenter>();
    }
}