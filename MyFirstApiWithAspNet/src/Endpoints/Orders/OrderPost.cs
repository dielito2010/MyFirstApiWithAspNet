using MyFirstApiWithAspNet.Domain.Orders;
using MyFirstApiWithAspNet.Endpoints.Orders;

namespace MyFirstApiWithAspNet.Endpoints.Oders;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(
        OrderRequest orderRequest, ApplicationDbContext context, HttpContext http)
    {
        var clientIdClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientNameClaim = http.User.Claims.First(c => c.Type == "Name").Value;

        List<Product> productsFound = null;
        if(orderRequest.ProductIds != null && orderRequest.ProductIds.Any())
        {
            productsFound = context.Products.Where(p => orderRequest.ProductIds.Contains(p.Id)).ToList();
        }

        var order = new Order(clientIdClaim, clientNameClaim, orderRequest.DeliveryAddress, productsFound);
        if (!order.IsValid)
        {
            return Results.ValidationProblem(order.Notifications.ConvertToProblemsDatails());
        }
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}
