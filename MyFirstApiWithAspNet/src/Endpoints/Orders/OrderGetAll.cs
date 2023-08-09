namespace MyFirstApiWithAspNet.Endpoints.Oders;

public class OrderGetAll
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext context, HttpContext http, int page = 1, int row = 10)
    {
        var errors = ValidateInputPageRows.Validate(page, row);
        if (errors.Length > 0)
            return Results.BadRequest(errors);

        var employeeClaim = http.User.Claims.FirstOrDefault(c => c.Type == "EmployeeCode");

        if (employeeClaim == null)
        {
            return Results.Problem
                (title: "Forbid: You do not have the necessary role to access this resource.", statusCode: 403);
        }
        else
        {
            List<OrderResponse> orderResponses;
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
            .Skip((page - 1) * row)
            .Take(row)
            .ToListAsync();
            return Results.Ok(orderResponses);
        }
    }
}




        /*var clientIdClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientCpfClaim = http.User.Claims.FirstOrDefault(c => c.Type == "Cpf").Value;

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
    }*/