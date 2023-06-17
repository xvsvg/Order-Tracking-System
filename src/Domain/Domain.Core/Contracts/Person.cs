using Domain.Core.ValueObjects;

namespace Domain.Core.Contracts;
#pragma warning disable CS8618

public abstract class Person
{
    private readonly List<ContactInfo> _contactInfo;

    protected Person()
    {
        _contactInfo = new List<ContactInfo>();
    }

    protected Person(FullName fullName, params ContactInfo[] contactInfo)
    {
        ArgumentNullException.ThrowIfNull(fullName);
        ArgumentNullException.ThrowIfNull(contactInfo);

        FullName = fullName;
        _contactInfo = new List<ContactInfo>(contactInfo);

        PersonId = Guid.NewGuid();
    }

    public Guid PersonId { get; }
    public FullName FullName { get; }
    public IEnumerable<ContactInfo> ContactInfo => _contactInfo;

    public ContactInfo AddContactInfo(ContactInfo contactInfo)
    {
        ArgumentNullException.ThrowIfNull(contactInfo);

        _contactInfo.Add(contactInfo);

        return contactInfo;
    }

    public void RemoveContactInfo(ContactInfo contactInfo)
    {
        ArgumentNullException.ThrowIfNull(contactInfo);

        _contactInfo.Remove(contactInfo);
    }
}