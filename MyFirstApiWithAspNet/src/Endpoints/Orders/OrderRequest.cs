namespace MyFirstApiWithAspNet.Endpoints.Orders;

public record OrderRequest(List<Guid> ProductIds, string DeliveryAddress);