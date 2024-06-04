namespace OnlineStore.Services.Utilities.Caching.InFile.Support.Serializer.Abstractions;

public interface IObjectSerializer
{
    string Serialize<TObject>(TObject obj)
        where TObject : notnull;

    TObject Deserialize<TObject>(string input)
        where TObject : notnull;

    TObject Deserialize<TObject>(string[] input)
        where TObject : notnull;

    TObject? DeserializeByKey<TObject>(object key, string input)
        where TObject : notnull;
}