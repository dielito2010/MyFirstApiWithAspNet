namespace MyFirstApiWithAspNet.Endpoints.Oders;

public record OrderResponse(Guid Id, string ClientName, string DeliveryAddress, List<string> Products, decimal Total);
