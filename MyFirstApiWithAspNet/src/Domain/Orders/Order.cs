using Flunt.Validations;

namespace MyFirstApiWithAspNet.Domain.Orders;

public class Order : Entity
{
    public string ClientId { get; private set; }
    public string DeliveryAddress { get; private set; }
    public decimal Total { get; private set; }
    public List<Product> Products { get; private set; }

    private Order() { }

    public Order(string clientId, string clientName, string deliveryAddress, List<Product> products)
    {
        ClientId = clientId;
        DeliveryAddress = deliveryAddress;
        Products = products;
        CreatedBy = clientName;
        EditedBy = clientName;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Total = 0;
        foreach(var item in Products)
        {
            Total += item.Price;
        }

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>()
            .IsNotNullOrEmpty(ClientId, "ClientId")
            .IsNotNullOrWhiteSpace(DeliveryAddress, "DeliveryAddress")
            .IsTrue(Products != null && Products.Any(), "Products");
        AddNotifications(contract);
    }
}
