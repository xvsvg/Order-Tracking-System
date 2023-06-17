using Domain.Core.Contracts;
using Domain.Core.ValueObjects;

namespace Domain.Core.Implementations;
#pragma warning disable CS8618

public class Customer : Person
{
    private readonly List<Order> _orderHistory;

    protected Customer()
    {
        _orderHistory = new List<Order>();
    }

    public Customer(
        FullName fullName,
        params ContactInfo[] contactInfo) : base(fullName, contactInfo)
    {
        _orderHistory = new List<Order>();
    }

    public virtual IEnumerable<Order> OrderHistory => _orderHistory;

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