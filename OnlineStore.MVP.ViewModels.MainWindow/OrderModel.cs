namespace OnlineStore.MVP.ViewModels.MainWindow;

public sealed class OrderModel()
{
    public Guid Id { get; init; }

    public required Guid UserId { get; init; }

    public required ICollection<ProductModel> Products { get; init; }

    public required decimal TotalAmount { get; init; }

    public required DateTime OrderDate { get; init; }

    public required string Status { get; init; }
}