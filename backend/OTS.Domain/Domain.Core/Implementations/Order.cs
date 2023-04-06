namespace OTS.Domain.Domain.Core.Implementations;

public class Order
{
    protected Order() {}

    public Order(DateTime dispatchDate, DateTime? deliveryDate, Courier? courier)
    {
        ArgumentNullException.ThrowIfNull(dispatchDate);

        DispatchDate = dispatchDate;
        DeliveryDate = deliveryDate;
        Courier = courier;

        OrderId = Guid.NewGuid();
    }

    public Guid OrderId { get; }
    public DateTime DispatchDate { get; }
    public DateTime? DeliveryDate { get; set; }
    public Courier? Courier { get; set; }
}
