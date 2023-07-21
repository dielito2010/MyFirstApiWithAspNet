using Microsoft.AspNetCore.Identity;
using MyFirstApiWithAspNet.Domain.Products;
using MyFirstApiWithAspNet.Endpoints.Categories;
using MyFirstApiWithAspNet.Endpoints.Employees;
using MyFirstApiWithAspNet.Infra.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("IWantDb");
builder.Services.AddSqlServer<ApplicationDbContext>(connectionString);
//builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:IWantDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle);


app.Run();
