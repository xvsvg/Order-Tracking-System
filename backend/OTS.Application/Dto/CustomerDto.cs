namespace OTS.Application.Dto;

public class CustomerDto
{
    public CustomerDto(Guid id, string name, IEnumerable<string> contactInfo, List<OrderDto> history)
    {
        Id = id;
        Name = name;
        History = history;
        ContactInfo = contactInfo.ToList();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<OrderDto> History { get; set; }
    public List<string> ContactInfo { get; set; }
}