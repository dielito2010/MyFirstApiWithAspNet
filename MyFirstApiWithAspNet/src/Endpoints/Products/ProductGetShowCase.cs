namespace MyFirstApiWithAspNet.Endpoints.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(int? page, int? rows, string? orderBy, ApplicationDbContext context)
    {
        var errors = ValidateInputPageRows.Validate(page, rows);

        if (errors.Length > 0)
            return Results.BadRequest(errors);
        if (string.IsNullOrEmpty(orderBy))
            orderBy = "name";

        var queryBase = context.Products.Include(p => p.Category).Where(p => p.HasStock && p.Category.Active);
        if (orderBy == "price")
            queryBase = queryBase.OrderBy(p => p.Price);

        var queryFilter = queryBase.Skip((page.Value - 1) * rows.Value).Take(rows.Value);

        var products = await queryFilter.ToListAsync();

        var results = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.Active));

        return Results.Ok(results);
    }
}


