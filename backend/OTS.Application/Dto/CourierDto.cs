namespace OTS.Application.Dto;

public class CourierDto
{
    public CourierDto(Guid id, string name, IEnumerable<string> contactInfo, Guid? customerId)
    {
        Id = id;
        Name = name;
        CustomerId = customerId;
        ContactInfo = contactInfo.ToList();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? CustomerId { get; set; }
    public List<string> ContactInfo { get; set; }
}