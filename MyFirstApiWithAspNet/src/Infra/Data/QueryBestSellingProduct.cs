using Dapper;
using Microsoft.Data.SqlClient;
using MyFirstApiWithAspNet.Endpoints.Reports;

namespace MyFirstApiWithAspNet.Infra.Data;

public class QueryBestSellingProduct
{
    public async Task<IEnumerable<BestSellingProductReportResponse>> Execute(int page, int rows)
    {
        var connectionString = Environment.GetEnvironmentVariable("IWantDb");

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Variável de ambiente não encontrada nesse servidor!");
            //return Results.BadRequest("Não autorizada a conexão com o banco de dados");
        }

        var query =
          @"SELECT p.Id, p.Name AS ProductName, COUNT(op.ProductsId) AS TotalSold, SUM(pr.Price) AS TotalRevenue
            FROM Products p
            INNER JOIN OrderProducts op ON p.Id = op.ProductsId
            INNER JOIN Orders o ON op.OrdersId = o.Id
            INNER JOIN Products pr ON p.Id = pr.Id
            GROUP BY p.Id, p.Name
            ORDER BY TotalRevenue DESC";

        using (var db = new SqlConnection(connectionString))
        {
            await db.OpenAsync();

            var result = await db.QueryAsync<BestSellingProductReportResponse>(
                query,
                new { page, rows }
            );

            return result;
        }
    }
}
