namespace MyFirstApiWithAspNet.Endpoints;

public class ValidateInputPageRows
{
    public static string[] Validate(int? page, int? rows)
    {
        var errors = new List<string>();

        if (page == null || page <= 0)
        {
            errors.Add("Page não pode ser nulo e tem que ser maior que 0.");
        }

        if (rows == null)
        {
            errors.Add("Rows não pode ser nulo.");
        }
        else if (rows < 1 || rows > 100)
        {
            errors.Add("Rows deve estar entre 1 e 100.");
        }

        return errors.ToArray();
    }
}