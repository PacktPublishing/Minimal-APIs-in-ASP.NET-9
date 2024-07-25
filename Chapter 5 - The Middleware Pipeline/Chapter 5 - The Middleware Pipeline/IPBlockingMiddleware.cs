namespace Chapter_5___The_Middleware_Pipeline
{
    public class IPBlockingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HashSet<string> _blockedIPs;
        public IPBlockingMiddleware(RequestDelegate next, IEnumerable<string> blockedIPs)
        {
            _next = next;
            _blockedIPs = new HashSet<string>(blockedIPs);
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var requestIP = context.Connection.RemoteIpAddress?.ToString();
            if (_blockedIPs.Contains(requestIP))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Your IP is blocked.");
                return;
            }
            await _next(context);
        }
    }
}
