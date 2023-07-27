namespace MyFirstApiWithAspNet.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]

    public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {
        var errors = ValidateInputPageRows.Validate(page, rows);

        if (errors.Length > 0)
        {
            return Results.BadRequest(errors);
            //Console.WriteLine(errors);
        }

        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }
}