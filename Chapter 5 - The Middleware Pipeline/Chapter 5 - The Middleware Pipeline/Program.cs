namespace Chapter_5___The_Middleware_Pipeline
{
    public class Program
    {
        private static readonly List<string> _blockedIPs = new List<string> { "192.168.1.1", "203.0.113.0" };

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            app.UseMiddleware<MySuperSimpleMiddlewareClass>();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<IPBlockingMiddleware>(_blockedIPs); 
            app.Use(async (context, next) =>
            {
                Console.WriteLine("Request handled by inline middleware component");
                await next(context);
                Console.WriteLine("Response handled by inline middleware component");
            });
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next(context);
                Console.WriteLine($"Response: {context.Response.StatusCode}");
            });

            app.MapGet("/employees/exceptionexample", () =>
            {
                throw new NotImplementedException();
            });

            app.Run();
        }
    }
}
