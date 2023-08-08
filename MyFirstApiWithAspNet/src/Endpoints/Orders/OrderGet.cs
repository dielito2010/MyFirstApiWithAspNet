namespace MyFirstApiWithAspNet.Endpoints.Oders;

public class OrderGet
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext context, HttpContext http)
    {
        var clientIdClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        string clientCpfClaim = null;

        var cpfClaim = http.User.Claims.FirstOrDefault(c => c.Type == "Cpf");

        if (cpfClaim != null)
        {
            clientCpfClaim = cpfClaim.Value;
        }

        List<OrderResponse> orderResponses;

        if (string.IsNullOrEmpty(clientCpfClaim))
        {
            orderResponses = await context.Orders
                .Where(order => order.Products.Any())
                .Join(context.Users,
                    order => order.ClientId,
                    user => user.Id,
                    (order, user) => new OrderResponse(
                        order.Id,
                        user.UserName,
                        order.DeliveryAddress,
                        order.Products.Select(p => p.Name).ToList(),
                        order.Total))
                .ToListAsync();
        }
        else
        {
            orderResponses = await context.Orders
                .Where(order => order.ClientId == clientIdClaim && order.Products.Any())
                .Join(context.Users,
                    order => order.ClientId,
                    user => user.Id,
                    (order, user) => new OrderResponse(
                        order.Id,
                        user.UserName,
                        order.DeliveryAddress,
                        order.Products.Select(p => p.Name).ToList(),
                        order.Total))
                .ToListAsync();
        }

        return Results.Ok(orderResponses);
    }

}