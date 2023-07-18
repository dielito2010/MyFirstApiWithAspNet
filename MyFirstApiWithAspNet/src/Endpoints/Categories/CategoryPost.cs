using MyFirstApiWithAspNet.Domain.Products;
using MyFirstApiWithAspNet.Infra.Data;

namespace MyFirstApiWithAspNet.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = new Category(categoryRequest.Name, "teste", "teste");

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemsDatails());
        }

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
