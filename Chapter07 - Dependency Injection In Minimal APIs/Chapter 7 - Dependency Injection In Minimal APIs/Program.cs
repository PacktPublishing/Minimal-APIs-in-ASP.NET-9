using Microsoft.AspNetCore.Mvc;
namespace Chapter_7___Dependency_Injection_In_Minimal_APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ProductRetrievalService>();
            builder.Services.AddSingleton<DeliveryDateBookingService>();
            
            var app = builder.Build();
            
            app.Run();
            
            app.MapGet("/getProductById/{id}", (int id, [FromServices] ProductRetrievalService productRetreivalService) =>
            {
                var productRepository = new ProductRepository(productRetreivalService);
                return Results.Ok(productRepository.Products.FirstOrDefault(x => x.Id == id));
            });
            
            app.MapPost("/order", (Order order, [FromServices] DeliveryDateBookingService deliveryDateBookingService) =>
            {
                order.DeliveryDate = deliveryDateBookingService.GetNextAvailableDate();
                // save order to repository in same way we did for Product using ProductRepository 
            });
            
            //ANTIPATTERN - AVOID! -----------------------------------------------------------------------
            app.MapPost("/order", (Order order, IServiceProvider provider) =>
            {
                var deliveryDateBookingService = provider.GetService<DeliveryDateBookingService>();
                order.DeliveryDate = deliveryDateBookingService.GetNextAvailableDate();
                // save order to repository in same way we did for Product using ProductRepository 
            });
            //--------------------------------------------------------------------------------------------
        }
    }
}
