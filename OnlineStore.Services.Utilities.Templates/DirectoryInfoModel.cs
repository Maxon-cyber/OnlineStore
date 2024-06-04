namespace OnlineStore.Services.Utilities.Templates;

public sealed class DirectoryInfoModel
{
    private readonly DirectoryInfo _directoryInfo;
    private readonly List<FileInfoModel> _files;

    public string Name => _directoryInfo.Name;

    public string FullName => _directoryInfo.FullName;

    public DateTime LastAccessTime => _directoryInfo.LastAccessTime;

    public DateTime LastWriteTime => _directoryInfo.LastWriteTime;

    public DirectoryInfoModel(string path)
    {
        _directoryInfo = new DirectoryInfo(path);

        FileInfo[] files = _directoryInfo.GetFiles();
        _files = [];

        foreach (FileInfo file in files)
            _files.Add(new FileInfoModel(file.FullName));

        _directoryInfo.Refresh();
    }

    public FileInfoModel? this[string shortFileName]
    {
        get
        {
            foreach (FileInfoModel file in _files)
                if (string.Equals(file.Name, shortFileName, StringComparison.CurrentCultureIgnoreCase))
                    return file;

            _directoryInfo.Refresh();

            return null;
        }
    }

    public IReadOnlyCollection<FileInfoModel> GetFiles()
        => _files.AsReadOnly();

    public static bool Exists(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return Directory.Exists(path);
    }
}