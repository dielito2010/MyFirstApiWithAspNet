namespace MyFirstApiWithAspNet.Endpoints.Reports;

public record BestSellingProductReportResponse(Guid Id, string ProductName, int TotalSold, decimal TotalRevenue);

