namespace OnlineStore.Services.Utilities.Templates;

public enum WriteMode
{
    Append = 0,
    WriteAll = 1,
}

public sealed class FileInfoModel
{
    private readonly FileInfo _fileInfo;

    public string Name => Path.GetFileNameWithoutExtension(_fileInfo.Name);

    public string FullName => _fileInfo.FullName;

    public long Size => _fileInfo.Length;

    public string Extension => _fileInfo.Extension;

    public DateTime LastAccessTime => _fileInfo.LastAccessTime;

    public DateTime LastWriteTime => _fileInfo.LastWriteTime;

    public FileInfoModel(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        _fileInfo = new FileInfo(path);
    }

    public async Task<string[]> ReadAsync()
    {
        string[] content = await File.ReadAllLinesAsync(FullName);

        return content;
    }

    public async Task<string[]> ReadByAsync(string key, string endString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        FileStream fileStream = new FileStream(FullName, FileMode.Open);

        if (fileStream.Length == 0)
            return [];

        List<string> lines = [];

        using StreamReader streamReader = new StreamReader(fileStream);

        bool keyIsFound = false;

        for (string line = string.Empty; line != null; line = await streamReader.ReadLineAsync())
        {
            if (line.Trim().StartsWith(key))
                keyIsFound = true;

            if (keyIsFound)
            {
                lines.Add(line);
                if (line.Trim() == endString)
                    break;
            }
        }

        return [.. lines];
    }

    public async Task WriteAsync(string content, WriteMode writeMode)
    {
        switch (writeMode)
        {
            case WriteMode.Append:
                await File.AppendAllTextAsync(FullName, content + "\n");
                _fileInfo.Refresh();
                break;
            case WriteMode.WriteAll:
                await File.WriteAllTextAsync(FullName, content);
                _fileInfo.Refresh();
                break;
            default:
                break;
        }
    }

    public async Task WriteAsync(string[] content, WriteMode writeMode)
    {
        switch (writeMode)
        {
            case WriteMode.Append:
                await File.AppendAllLinesAsync(FullName, content);
                _fileInfo.Refresh();
                break;
            case WriteMode.WriteAll:
                await File.WriteAllLinesAsync(FullName, content);
                _fileInfo.Refresh();
                break;
            default:
                break;
        }
    }

    public async Task ClearAsync()
    {
        await File.WriteAllTextAsync(FullName, string.Empty);
        _fileInfo.Refresh();
    }

    public static bool Exists(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return File.Exists(path);
    }
}