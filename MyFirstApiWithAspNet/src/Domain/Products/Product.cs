﻿using Flunt.Validations;

namespace MyFirstApiWithAspNet.Domain.Products;

public class Product : Entity
{
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public decimal Price { get; private set; }
    public bool Active { get; private set; } = true;

    private Product(string name) { }

    public Product(string name, Category category, string description, bool hasStock, string createdBy, decimal price)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;
        Price = price;

        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();
        
    }

    public Product(string name, Category? category, string description, bool hasStock, decimal price, string userId)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;
        Price = price;
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsNotNullOrEmpty(Description, "Description")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(EditedBy, "EditedBy")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsGreaterOrEqualsThan(Price, 0.01, "Price")
            .IsGreaterOrEqualsThan(Description, 3, "Description")
            .IsNotNull(Category, "Category", "Category is not found or null");
          AddNotifications(contract);
    }
}
