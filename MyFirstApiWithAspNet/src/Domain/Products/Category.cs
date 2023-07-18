using Flunt.Validations;

namespace MyFirstApiWithAspNet.Domain.Products;

public class Category : Entity
{
    public string Name { get; private set; }
    public bool Active { get; private set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        
        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrWhiteSpace(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrWhiteSpace(CreatedBy, "cretedBy")
            .IsNotNullOrWhiteSpace(EditedBy, "editedBy");
        AddNotifications(contract);
    }

    public void EditInfo(string name, bool active)
    {
        Active = active;
        Name = name;

        Validate();
    }

}
