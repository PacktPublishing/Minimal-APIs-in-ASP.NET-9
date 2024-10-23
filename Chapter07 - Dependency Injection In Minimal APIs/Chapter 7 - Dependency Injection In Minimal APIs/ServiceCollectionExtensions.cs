namespace Chapter_7___Dependency_Injection_In_Minimal_APIs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IMyService, MyService>();
            services.AddSingleton<IOtherService, OtherService>();
            return services;
        }
    }

    //Interfaces and classes for demonstration purposes only
    public interface IMyService { }
    public interface IOtherService { }
    public class MyService : IMyService { }
    public class OtherService : IOtherService { }

    //with the above extension method implemented, you should be able to replace multiple lines of code in Main() of Program.cs with one line to AddMyServices();
    // for example - builder.Services.AddMyServices(); 
}
