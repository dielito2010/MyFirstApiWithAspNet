namespace MyFirstApiWithAspNet.Endpoints.Clients;

public class ClientGet
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(HttpContext http)
    {
        var user = http.User;

        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var nameClaim = user.Claims.FirstOrDefault(c => c.Type == "Name");
        var cpfClaim = user.Claims.FirstOrDefault(c => c.Type == "Cpf");

        if (idClaim != null && nameClaim != null && cpfClaim != null)
        {
            var results = new
            {
                Id = idClaim.Value,
                Name = nameClaim.Value,
                Cpf = cpfClaim.Value,
            };
            return Results.Ok(results);
        }
        else
        {
            return Results.BadRequest("User information not available.");
        }
    }
}
