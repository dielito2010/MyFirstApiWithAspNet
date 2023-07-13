using Flunt.Validations;

namespace MyFirstApiWithAspNet.Domain.Products;

public class Category : Entity
{
    public string Name { get; set; }
    public bool Active { get; set; }

    public Category(string name, string createdBy, string editedBy)
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "Name", "Nome é obrigatório!")
            .IsNotNullOrEmpty(name, "cretedBy", "Criador é obrigatório!")
            .IsNotNullOrEmpty(name, "editedBy", "Editor é obrigatório!");
        AddNotifications(contract);


        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
    }

}
