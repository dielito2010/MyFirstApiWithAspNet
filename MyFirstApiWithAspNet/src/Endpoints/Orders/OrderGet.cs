namespace MyFirstApiWithAspNet.Endpoints.Oders;

public class OrderGet
{
    public static string Template => "/orders/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action
        (Guid id, ApplicationDbContext context, HttpContext http, UserManager<IdentityUser> userManager)
    {
        var IdClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        var employeeClaim = http.User.Claims.FirstOrDefault(c => c.Type == "EmployeeCode");

        var order = context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);

        if (order.ClientId != IdClaim.Value && employeeClaim == null)
            return Results.Problem
                (title: "Forbid: You do not have the necessary role to access this resource.", statusCode: 403);

        var client = await userManager.FindByIdAsync(order.ClientId);
        var productsResponse = order.Products.Select(p => p.Name).ToList();
        var orderResponse = new OrderResponse
            (order.Id, client.UserName, order.DeliveryAddress, productsResponse, order.Total);

        return Results.Ok(orderResponse);
    }
}