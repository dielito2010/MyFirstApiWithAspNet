namespace MyFirstApiWithAspNet.Endpoints.Employees;

public record ClientRequest(string Cpf, string Name, string Email, string Password);