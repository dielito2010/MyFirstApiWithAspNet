namespace MyFirstApiWithAspNet.Endpoints.Reports;

public class BestSellingProductReportGet
{
    public static string Template => "/reports/best-selling-products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(policy: "EmployeePolicy")]
    public static async Task<IResult> Action(QueryBestSellingProduct query, int page = 1, int rows =10)
    {
        var errors = ValidateInputPageRows.Validate(page, rows);

        if (errors.Length > 0)
        {
            return Results.BadRequest(errors);
        }

        var result = await query.Execute(page, rows);
        return Results.Ok(result);
    }
}