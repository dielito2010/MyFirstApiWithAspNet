using Dapper;
using Microsoft.Data.SqlClient;

namespace MyFirstApiWithAspNet.Infra.Data;

public class QueryAllUsersWithClaimName
{
    public async Task<IEnumerable<EmployeeResponse>> Execute(int page, int rows)
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

        using (var db = new SqlConnection(connectionString))
        {
            await db.OpenAsync();

            var result = await db.QueryAsync<EmployeeResponse>(
                query,
                new { page, rows }
            );

            return result;
        }
    }
}
