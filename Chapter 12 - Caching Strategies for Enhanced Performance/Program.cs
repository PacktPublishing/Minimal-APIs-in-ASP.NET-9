using Chapter_9___Entity_Framework_Core_and_Dapper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Caching.Memory;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Text.Json;


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
            builder.Services.AddResponseCaching();
            builder.Services.AddMemoryCache();
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

            //IMemoryCache Example
            app.MapGet("/employees/{id}", async (int id, [FromServices] DapperService dapperService, 
                IMemoryCache memoryCache
                ) =>
            {

                if (memoryCache.TryGetValue(id, out var result))
                {
                    return result;
                }
                var employee = await dapperService.GetEmployeeById(id);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30));
                memoryCache.Set<Employee>(employee.Id, employee, cacheEntryOptions);

                return Results.Ok(employee);
            });

            //Response caching example
            app.MapGet("/employees/{id}", async (int id, [FromServices] DapperService dapperService,
                HttpContext context
                ) =>
            {
                var employee = await dapperService.GetEmployeeById(id);
                context.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(60)
            };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                return Results.Ok(employee);
            });

            //redis example
            app.MapGet("/employees/{id}", async (int id, [FromServices] DapperService dapperService) =>
            {
                ConfigurationOptions options = new ConfigurationOptions
                {
                    EndPoints = { { "192.168.2.8", 6379 } },

                };

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options);
                IDatabase db = redis.GetDatabase();
                var employeeIdKey = id.ToString();
                var cachedEmployee = db.StringGet(employeeIdKey);

                if (cachedEmployee.HasValue)
                {
                    return Results.Ok(JsonSerializer.Deserialize<Employee>(cachedEmployee));
                }

                var employee = await dapperService.GetEmployeeById(id);
                db.StringSet(employeeIdKey, JsonSerializer.Serialize(employee));
                return Results.Ok(employee);
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
