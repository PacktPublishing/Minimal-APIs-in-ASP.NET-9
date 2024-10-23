using Chapter_9___Entity_Framework_Core_and_Dapper.Models;
using Microsoft.AspNetCore.Mvc;


namespace Chapter_9___Entity_Framework_Core_and_Dapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<DapperService>();
            builder.Services.AddScoped<MyCompanyContext>();
            builder.Services.AddScoped<EmployeeService>();
            var app = builder.Build();

            app.MapPost("/employees", async (Employee employee, [FromServices] DapperService dapperService) =>
            {
                await dapperService.AddEmployee(employee);
                return Results.Created();
            });

            app.MapPost("/employees", async (Models.Employee employee, [FromServices] EmployeeService employeeService) =>

            {
                await employeeService.AddEmployee(employee);
                return Results.Created();
            });

            app.MapGet("/employees/{id}", async (int id, [FromServices] DapperService dapperService) =>
            {
                return Results.Ok(await dapperService.GetEmployeeById(id));
            });

            app.MapPut("/employees", async (Employee employee, [FromServices] DapperService dapperService) =>
            {
                await dapperService.UpdateEmployee(employee);
                return Results.Ok();
            });

            app.MapDelete("/employees/{id}", async (int id, [FromServices] DapperService dapperService) =>
            {
                await dapperService.DeleteEmployeeById(id);
                return Results.NoContent();
            });

            app.Run();
        }
    }
}
