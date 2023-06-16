namespace Application.Dto;

public class OrderDto
{
    public OrderDto(Guid id, CourierDto courier)
    {
        Id = id;
        Courier = courier;
    }

    public Guid Id { get; set; }
    public CourierDto Courier { get; set; }
}