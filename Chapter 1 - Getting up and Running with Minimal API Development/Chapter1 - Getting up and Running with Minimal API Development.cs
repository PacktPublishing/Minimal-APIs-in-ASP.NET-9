
//Hello world in Minimal API
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet(“/”, () => “Hello World!”);
app.Run();

//Example of ASP.NET Controller
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
            private readonly IEmployeeRepository     _employeeRepository;

    public EmployeesController(IEmployeeRepository                       employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetEmployees()
    {
        var employees = _employeeRepository.GetEmployees();
        return Ok(employees);
    }
 
      [HttpPost]
      public ActionResult<Employee> CreateEmployee(Employee   employee)
        {
            _employeeRepository.AddEmployee(employee);
           return CreatedAtAction(nameof(GetEmployees), 
                   new{id = employee.Id }, employee);
        }
    }
}

//Creating a Minimal API endpoint in Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

        var app = builder.Build();

        app.MapGet("/api/employees", (IEmployeeRepository employeeRepository) =>
        {
            var employees = employeeRepository.GetEmployees();
            return Results.Ok(employees);
        });
        app.Run();
    }
}
