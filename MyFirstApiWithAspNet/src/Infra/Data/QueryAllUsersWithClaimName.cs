using Dapper;
using Microsoft.Data.SqlClient;
using MyFirstApiWithAspNet.Endpoints;
using MyFirstApiWithAspNet.Endpoints.Employees;

namespace MyFirstApiWithAspNet.Infra.Data;

public class QueryAllUsersWithClaimName
{
    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var connectionString = Environment.GetEnvironmentVariable("IWantDb");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Variável de ambiente não encontrada nesse servidor!");
            //return Results.BadRequest("Não autorizada a conexão com o banco de dados");
        }

        var query =
            @"select Email, ClaimValue as Name
                from AspNetUsers u inner join AspNetUserClaims c
                on u.id = c.UserId and claimtype = 'Name'
                order by name
                OFFSET (@page -1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var db = new SqlConnection(connectionString);
        return db.Query<EmployeeResponse>(
            query,
            new { page, rows }
        );
    }
}
