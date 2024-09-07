namespace Chapter_8___Integrating_Minimal_APIs_with_Data_Sources
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IDatabaseService, SqlService>();
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddSingleton<IDatabaseService>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var useMongoDB = config.GetValue<bool>("UseMongoDB");
                if (useMongoDB)
                {
                    return sp.GetRequiredService<MongoDbService>();
                }
                else
                {
                    return sp.GetRequiredService<SqlService>();
                }
            });

            var app = builder.Build();

            app.MapGet("/employees", async (IDatabaseService dbService) =>
            {
                var employees = await dbService.GetEmployeesAsync();
                return Results.Ok(employees);
            });

            app.MapPost("/employees", async (IDatabaseService dbService, Employee employee) =>
            {
                await dbService.AddEmployeeAsync(employee);
                return Results.Created($"/employees/{employee.Id}", employee);
            });

            app.Run();
        }
    }
}
