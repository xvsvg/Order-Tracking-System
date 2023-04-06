using OTS.Domain.Domain.Core.Contracts;
using OTS.Domain.Domain.Core.ValueObjects;
#pragma warning disable CS8618

namespace OTS.Domain.Domain.Core.Implementations;

public class Customer : Person
{
    private readonly List<Order> _orderHistory;

    protected Customer() { }

    public Customer(
        AccountCredentials accountCredentials,
        FullName fullName,
        params ContactInfo[] contactInfo) : base(fullName, contactInfo)
    {
        ArgumentNullException.ThrowIfNull(accountCredentials);

        AccountCredentials = accountCredentials;

        _orderHistory = new List<Order>();
    }

    public AccountCredentials AccountCredentials { get; }
    public IEnumerable<Order> OrderHistory => _orderHistory;

    public Order AddOrderToHistory(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _orderHistory.Add(order);

        return order;
    }

    public void RemoveFromOrderHistory(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);


        _orderHistory.Remove(order);
    }
}