namespace OnlineStore.MVP.ViewModels.MainWindow;

public sealed class ProductModel()
{
    private object _image;

    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required object Image
    {
        get => _image;
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            Type typeOfValue = value.GetType();
            if (typeOfValue != typeof(string) && typeOfValue != typeof(byte[]))
                throw new ArgumentNullException(nameof(value), "Тип данных для фотогорафии должен быть либо строка(путь), либо массив байт(сконфертированная фотография в массив байт)");

            if (typeOfValue == typeof(string))
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(value.ToString());

                if (Path.IsPathRooted(value.ToString()))
                    _image = value;
            }
            else if (typeOfValue == typeof(byte[]))
            {
                byte[] bytes = (value as byte[])!;

                if (bytes.Length > 2 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF || bytes.Length > 7 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E &&
                    bytes[3] == 0x47 && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A)
                    _image = value;
            }
        }
    }

    public required int Quantity { get; init; }

    public required string Category { get; init; }

    public required decimal Price { get; init; }
}