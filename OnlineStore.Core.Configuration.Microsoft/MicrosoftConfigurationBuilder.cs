using Microsoft.Extensions.Configuration;
using OnlineStore.Core.Configuration.Abstractions;

namespace OnlineStore.Core.Configuration.Microsoft;

public sealed class MicrosoftConfigurationBuilder : IApplicationConfigurationBuilder
{
    private static bool _isBuilded = false;
    private static IConfigurationRoot _root;
    private readonly IConfigurationBuilder _configurationBuilder;

    public IConfigurationRoot Root => _root;

    public MicrosoftConfigurationBuilder()
        => _configurationBuilder = new ConfigurationManager();

    public IApplicationConfigurationBuilder SetBasePath(string basePath)
    {
        _configurationBuilder.SetBasePath(basePath);
        return this;
    }

    public IApplicationConfigurationBuilder AddFile(string fileName, bool optional, bool reloadOnChange)
    {
        switch (Path.GetExtension(fileName))
        {
            case ".yml":
                _configurationBuilder.AddYamlFile(fileName, optional, reloadOnChange);
                break;
            default:
                throw new ArgumentException($"Провайдер конфигурации для файла {fileName} не найден");
        }

        return this;
    }

    public IApplicationConfigurationBuilder AddEnviromentVariables()
    {
        _configurationBuilder.AddEnvironmentVariables();
        return this;
    }

    public void Build()
    {
        if (_isBuilded)
            return;

        _root = _configurationBuilder.Build();
        _isBuilded = true;
    }
}