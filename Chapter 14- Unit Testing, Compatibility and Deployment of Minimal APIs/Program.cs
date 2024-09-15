namespace Chapter_14
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddTransient<CalculatorService>();
            var app = builder.Build();
            

            app.MapGet("/", () => "Hello World!");

            app.MapPost("/SumIntegers", (int[] integers, CalculatorService calculatorService) =>
            {
                var result = calculatorService.Sum(integers);
                return Results.Ok(result);
            });

            app.Run();
        }
    }
}
