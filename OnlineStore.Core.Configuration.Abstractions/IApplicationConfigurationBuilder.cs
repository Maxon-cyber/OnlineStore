namespace OnlineStore.Core.Configuration.Abstractions;

public interface IApplicationConfigurationBuilder
{
    IApplicationConfigurationBuilder SetBasePath(string basePath);

    IApplicationConfigurationBuilder AddFile(string fileName, bool optional, bool reloadOnChange);

    IApplicationConfigurationBuilder AddEnviromentVariables();

    void Build();
}