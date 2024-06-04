namespace OnlineStore.Services.Utilities.Logger.Abstractions;

public interface ILogger
{
    string MessagePattern { get; }

    string Separator { get; }

    Task LogInfoAsync(string message);

    Task LogErrorAsync(Exception exception, string message);

    Task LogMessageAsync(string message);

    Task LogSeparatorAsync();
}