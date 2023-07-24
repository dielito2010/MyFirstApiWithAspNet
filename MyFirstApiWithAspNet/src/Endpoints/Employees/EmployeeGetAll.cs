using Dapper;
using Microsoft.Data.SqlClient;
using MyFirstApiWithAspNet.Endpoints.Employees;
using System.ComponentModel.DataAnnotations;

namespace MyFirstApiWithAspNet.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows)
    {
        var errors = ValidateInputPageRows.Validate(page, rows);

        if (errors.Length > 0)
        {
            return Results.BadRequest(errors);
        }

        var connectionString = Environment.GetEnvironmentVariable("IWantDb");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Variável de ambiente não encontrada nesse servidor!");
            return Results.BadRequest("Não autorizada a conexão com o banco de dados");
        }

        var query =
            @"select Email, ClaimValue as Name
                from AspNetUsers u inner join AspNetUserClaims c
                on u.id = c.UserId and claimtype = 'Name'
                order by name
                OFFSET (@page -1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var db = new SqlConnection(connectionString);
        var employees = db.Query<EmployeeResponse>(
            query,
            new { page, rows }
            );
        return Results.Ok(employees);

    }
}