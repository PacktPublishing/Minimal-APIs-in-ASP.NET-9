using System.Collections.Concurrent;

namespace Chapter_11___Asynchronous_Programming
{
    public class Program
    {

        


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/fetch-data", (HttpContext httpContext) =>
            {
                HttpClient client = new HttpClient();
                string url = "https://jsonplaceholder.typicode.com/posts/1";

                // Initiate the asynchronous operation and return a continuation task
                return client.GetStringAsync(url).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        // Task completed successfully, return the data
                        return httpContext.Response.WriteAsJsonAsync(new { data = task.Result });
                    }
                    else if (task.IsFaulted)
                    {
                        // Task faulted, handle the exception
                        var errorMessage = task.Exception.Flatten().InnerException?.Message ?? "An error occurred";
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return httpContext.Response.WriteAsJsonAsync(new { error = errorMessage });
                    }
                    else
                    {
                        // If task was cancelled or some other state, handle accordingly
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return httpContext.Response.WriteAsJsonAsync(new { error = "Unknown error occurred." });
                    }
                });
            });

            var results = new ConcurrentDictionary<Guid, string>();

            // Endpoint to start the long-running background task
            app.MapPost("/start-process", async () =>
            {
                var requestId = Guid.NewGuid();
                var requestIdStr = requestId.ToString();

                // Start the long-running task
                _ = Task.Run(async () =>
                {
                    await Task.Delay(10000); // Simulate a long-running task (10 seconds)
                    results[requestId] = $"Result for {requestIdStr}"; // Store result in dictionary
                });

                // Respond with the request ID
                return Results.Ok(new { RequestId = requestIdStr });
            });

            // Endpoint to get the result based on the request ID
            app.MapGet("/get-result/{requestId}", (string requestId) =>
            {
                if (Guid.TryParse(requestId, out var guid) && results.TryGetValue(guid, out var result))
                {
                    return Results.Ok(new { Result = result });
                }
                return Results.NotFound(new { Error = "Result not found or not yet completed." });
            });

            app.MapGet("/fetch-data-async-await", async (HttpContext httpContext) =>
            {
                HttpClient client = new HttpClient();
                string url = "https://jsonplaceholder.typicode.com/posts/1";

                try
                {
                    // Asynchronously fetch data from the external service
                    string data = await client.GetStringAsync(url);
                    await httpContext.Response.WriteAsJsonAsync(new { data });
                }
                catch (HttpRequestException ex)
                {
                    // Handle error (e.g., network issues, server problems)
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(new { error = "Error fetching data: " + ex.Message });
                }
                catch (Exception ex)
                {
                    // Handle any other exceptions
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred: " + ex.Message });
                }
            });

            app.Run();
        }

      
    }
}
