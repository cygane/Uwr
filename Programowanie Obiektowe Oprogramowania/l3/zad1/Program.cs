using System;
using System.Collections.Generic;
using System.Linq;

// information expert - Przydziel zobowiązanie “ekspertowi” – tej klasie, która ma informacje
// konieczne do jego realizacji.
class Order
{
    private List<OrderItem> items = new List<OrderItem>();

    public void AddItem(OrderItem item)
    {
        items.Add(item);
    }

    public double CalculateTotalPrice()
    {
        return items.Sum(item => item.GetTotalPrice()); // Order deleguje obliczenia do OrderItem
    }
}

class OrderItem
{
    public string Name { get; }
    public int Quantity { get; }
    public double UnitPrice { get; }

    public OrderItem(string name, int quantity, double unitPrice)
    {
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public double GetTotalPrice()
    {
        return Quantity * UnitPrice; // OrderItem jako ekspert informacyjny wie, jak obliczyć cenę
    }
}

// creator - Przydziel zobowiązanie tworzenia instancji klasy A klasie B, jeżeli zachodzi
// jeden z warunków:
// • B „zawiera” A lub agreguje A (kompozycja)
// • B zapamiętuje A
// • B bezpośrednio używa A
// • B posiada dane inicjalizacyjne dla A

class OrderFactory
{
    public static Order CreateOrderWithItems(List<OrderItem> items)
    {
        Order order = new Order();
        foreach (var item in items)
        {
            order.AddItem(item);
        }
        return order; // OrderFactory tworzy i inicjalizuje obiekt Order
    }
}

// Test działania
class Program
{
    static void Main()
    {
        List<OrderItem> items = new List<OrderItem>
        {
            new OrderItem("Laptop", 1, 3000),
            new OrderItem("Mysz", 2, 150)
        };

        Order order = OrderFactory.CreateOrderWithItems(items);
        Console.WriteLine("Łączna cena zamówienia: " + order.CalculateTotalPrice() + " PLN");
    }
}
