namespace OnlineStore.Entities.Order;

public enum Status
{
    None = 0,
    InProcessing = 1,
    SubmittedForAssembly = 2,
    Sorted = 3,
    DeliveredToTheCourier = 4,
    Delivered = 5
}