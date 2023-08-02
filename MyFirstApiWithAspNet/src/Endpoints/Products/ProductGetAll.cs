﻿namespace MyFirstApiWithAspNet.Endpoints.Products;

public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = await context.Products
        .Include(p => p.Category)
        .OrderBy(p => p.Name)
        .ToListAsync();

        var results = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Active));

        return Results.Ok(results);
    }
}


