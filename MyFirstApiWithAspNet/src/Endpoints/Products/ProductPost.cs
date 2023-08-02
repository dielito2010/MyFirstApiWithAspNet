namespace MyFirstApiWithAspNet.Endpoints.Products;

public class ProductPost
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ProductRequest productRequest, HttpContext http, ApplicationDbContext context)
    {
        bool productExists = context.Products.Any(c => c.Name == productRequest.Name);

        if (productExists)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "Name", new[] { "Já existe um produto com este nome." } }
        });
        }

        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == productRequest.CategoryId);
        var product =
            new Product(productRequest.Name, category, productRequest.Description, productRequest.HasStock, productRequest.Price, userId);

        if (!product.IsValid)
        {
            return Results.ValidationProblem(product.Notifications.ConvertToProblemsDatails());
        }

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return Results.Created($"/products/{product.Id}", product.Id);
    }
}
