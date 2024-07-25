namespace Chapter_5___The_Middleware_Pipeline
{
    public class MySuperSimpleMiddlewareClass
    {
        private readonly RequestDelegate _next;
        
        public MySuperSimpleMiddlewareClass(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Request handled by middleware component");
            await _next(context);
            Console.WriteLine("Response handled by middleware component");
        }
    }
}
