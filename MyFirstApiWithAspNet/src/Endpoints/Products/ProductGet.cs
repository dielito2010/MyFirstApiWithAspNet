namespace MyFirstApiWithAspNet.Endpoints.Products;

public class ProductGet
{
    public static string Template => "/products/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => new Func<Guid, ApplicationDbContext, Task<IResult>>(Action);

    public static async Task<IResult> Action(Guid id, ApplicationDbContext context)
    {
        try
        {
            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return Results.NotFound("Product not found");
            }

            var result = new ProductResponse(product.Name, product.Category.Name, product.Description, product.HasStock, product.Active);

            return Results.Ok(result);
        }
        catch (Exception)
        {
            return Results.Problem("Internal server error", statusCode: 500);
        }
    }
}




