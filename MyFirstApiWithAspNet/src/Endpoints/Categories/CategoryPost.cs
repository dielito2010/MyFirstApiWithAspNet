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
        var category = new Category(categoryRequest.Name)
        {
            CreatedBy = "teste",
            CreatedOn = DateTime.Now,
            EditedBy = "teste",
            EditedOn = DateTime.Now,
        };

        if (!category.IsValid)
            return Results.BadRequest(category.Notifications);

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
