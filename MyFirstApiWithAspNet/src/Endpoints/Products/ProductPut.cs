namespace MyFirstApiWithAspNet.Endpoints.Products;

public class ProductPut
{
    public static string Template => "/products/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(
       [FromRoute] Guid id, HttpContext http, ProductRequest categoryRequest, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null)
            return Results.NotFound(new Dictionary<string, string[]>
        {
            { "Id", new[] { "Categoria não encontrada no banco de dados." } }
        });

        category.EditInfo(categoryRequest.Name, categoryRequest.Active, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemsDatails());

        context.SaveChanges();

        return Results.Ok(category.Id);
    }
}
