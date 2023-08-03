namespace MyFirstApiWithAspNet.Endpoints.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int row = 10, string orderBy = "name")
    {
        var errors = ValidateInputPageRows.Validate(page, row);
        if (errors.Length > 0)
            return Results.BadRequest(errors);

        var queryBase = context.Products.AsNoTracking().Include(p => p.Category).Where(p => p.HasStock && p.Category.Active);
        if (orderBy == "price")
            queryBase = queryBase.OrderBy(p => p.Price);
        else if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else
            return Results.Problem(title: "Order only by price or name", statusCode: 400);

        var queryFilter = queryBase.Skip((page - 1) * row).Take(row);

        var products = await queryFilter.ToListAsync();

        var results = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.Active));

        return Results.Ok(results);
    }
}


