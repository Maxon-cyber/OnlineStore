using OnlineStore.Services.Utilities.Logger.Abstractions;
using OnlineStore.Services.Utilities.Templates;
using System.Collections;

namespace OnlineStore.Services.Utilities.Logger.InFile;

public sealed class FileLogger(string path) : ILogger
{
    private readonly FileInfoModel _fileInfo = new FileInfoModel(path);
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

    public DirectoryInfoModel Directory { get; }

    public string MessagePattern { get; } = "{0:yyyy-MM-dd HH:mm:ss} [{1}]: {2}";

    public string Separator { get; } = new string('=', 100);

    public long SizeLimit => 8_192L;

    public async Task LogInfoAsync(string message)
    {
        await _semaphore.WaitAsync();

        await Clear();

        await _fileInfo.WriteAsync(string.Format(MessagePattern, DateTime.Now, "Info", message), WriteMode.Append);

        _semaphore.Release();
    }

    public async Task LogErrorAsync(Exception exception, string message)
    {
        await _semaphore.WaitAsync();

        await Clear();

        await _fileInfo.WriteAsync(string.Format(MessagePattern, DateTime.Now, "Error", message), WriteMode.Append);

        await _fileInfo.WriteAsync($"Тип ошибки: {exception.GetType()}", WriteMode.Append);

        await _fileInfo.WriteAsync("Доп. данные об ошибке:", WriteMode.Append);

        string exceptionData = null!;
        foreach (DictionaryEntry data in exception.Data)
            exceptionData += $"\t{data.Key} - {data.Value}";

        await _fileInfo.WriteAsync(exceptionData, WriteMode.Append);

        _semaphore.Release();
    }

    public async Task LogSeparatorAsync()
    {
        await _semaphore.WaitAsync();

        await _fileInfo.WriteAsync(Separator, WriteMode.Append);

        _semaphore.Release();
    }

    public async Task LogMessageAsync(string message)
    {
        await _semaphore.WaitAsync();

        await Clear();

        await _fileInfo.WriteAsync(message, WriteMode.Append);

        _semaphore.Release();
    }

    private async Task Clear()
    {
        if (_fileInfo.Size >= SizeLimit)
            while (_fileInfo.Size > SizeLimit / 2)
            {
                string[] lines = await _fileInfo.ReadAsync();

                await _fileInfo.WriteAsync(lines.Skip(Array.IndexOf(lines, Separator) + 1).ToArray(), WriteMode.WriteAll);
            }
    }
}