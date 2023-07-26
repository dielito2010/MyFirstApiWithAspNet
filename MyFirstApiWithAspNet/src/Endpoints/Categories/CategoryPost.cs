using MyFirstApiWithAspNet.Domain.Products;
using MyFirstApiWithAspNet.Infra.Data;
using System.Security.Claims;

namespace MyFirstApiWithAspNet.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context)
    {
        // Verifica se já existe uma categoria com o mesmo nome no banco de dados
    bool categoryExists = context.Categories.Any(c => c.Name == categoryRequest.Name);

    if (categoryExists)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "Name", new[] { "Já existe uma categoria com este nome." } }
        });
    }

    var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    var category = new Category(categoryRequest.Name, userId, userId);

    if (!category.IsValid)
    {
        return Results.ValidationProblem(category.Notifications.ConvertToProblemsDatails());
    }

    context.Categories.Add(category);
    context.SaveChanges();

    return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
