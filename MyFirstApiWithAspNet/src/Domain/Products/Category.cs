using Flunt.Validations;

namespace MyFirstApiWithAspNet.Domain.Products;

public class Category : Entity
{
    public string Name { get; set; }
    public bool Active { get; set; }

    public Category(string name)
    {
        var contract = new Contract<Category>()
            .IsNotNull(name, "Name", "Nome é obrigatório!");
        AddNotifications(contract);


        Name = name;
        Active = true;
    }

}
