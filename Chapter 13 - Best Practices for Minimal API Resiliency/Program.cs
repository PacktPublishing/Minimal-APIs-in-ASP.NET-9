using Chapter_9___Entity_Framework_Core_and_Dapper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Caching.Memory;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;




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
            builder.Services.AddScoped<ProbationaryAnnualLeaveStrategy>();
            builder.Services.AddScoped<PostProbationaryAnnualLeaveStrategy>();
            builder.Services.AddMemoryCache();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://auth.yourdomain.com",
                        ValidAudience = "https://api.yourdomain.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A_Very_Secret_Key_12345"))
                    };

                });

            var app = builder.Build();

            app.MapGet("/generate-token", () =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("A_Very_Secret_Key_12345");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "Admin")
        }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "https://yourdomain.com",
                    Audience = "https://yourdomain.com",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Results.Ok(tokenString);
            });


            app.MapGet("/calculate-employee-leave-allowance/{employeeId}", async (int employeeId, bool employeeOnProbation, [FromServices] EmployeeService employeeService) =>
            {
                IAnnualLeaveStrategy annualLeaveStrategy = employeeOnProbation ? new ProbationaryAnnualLeaveStrategy() : new PostProbationaryAnnualLeaveStrategy();
                var employee = await employeeService.GetEmployeeById(employeeId);
                return annualLeaveStrategy.CalculateLeaveAllowance(employee);
            });



          



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

            app.MapGet("/employees/{id}", async (int id, [FromServices] DapperService dapperService, HttpContext context) =>
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
               
                var cache = app.Services.GetRequiredService<IMemoryCache>();
                if(cache.TryGetValue(id, out var result))
                {
                    return result;
                }
                
                
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30));
                cache.Set<Employee>(employee.Id, employee, cacheEntryOptions);

                //var employee = await dapperService.GetEmployeeById(id);
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
