namespace Chapter_2___Creating_your_first_Minimal_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            
            //Example GET Method
            app.MapGet("/employees/{id}", (int id) =>
            {
                var employee = EmployeeManager.Get(id);
                return Results.Ok(employee);
            });

            //Example POST Method
            app.MapPost("/employees", (Employee employee) =>
            {
                EmployeeManager.Create(employee);
                return Results.Created();
            });

            //Example PUT Method
            app.MapPut("/employees", (Employee employee) =>
            {
                EmployeeManager.Update(employee);
                return Results.Ok();
            });

            //Example PATCH Method
            app.MapPatch("/updateEmployeeName", (Employee employee) =>
            {
                EmployeeManager.ChangeName(employee.Id, employee.Name);
                return Results.Ok();
            });

            //Example DELETE Method
            app.MapDelete("/deleteEmployee/{id}", (int id) =>
            {
                EmployeeManager.Delete(id);
                return Results.Ok();
            });




            app.Run();
        }
    }
}
