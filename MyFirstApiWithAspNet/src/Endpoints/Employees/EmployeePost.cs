using MyFirstApiWithAspNet.Domain.Users;

namespace MyFirstApiWithAspNet.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(
        EmployeeRequest employeeRequest, UserCreator userCreator, HttpContext http)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            new Claim("CreatedBy", userId)
        };

        (IdentityResult identity, string userId) result =
            await userCreator.Create(employeeRequest.Email, employeeRequest.Password, userClaims);

        if (!result.identity.Succeeded)
            return Results.BadRequest(result.identity.Errors.First());

        return Results.Created($"/employees/{result.userId}", result.userId);
    }
}
