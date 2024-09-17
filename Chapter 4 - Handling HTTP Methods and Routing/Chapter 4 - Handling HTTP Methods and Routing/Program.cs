using System.ComponentModel.DataAnnotations;

namespace Chapter_4___Handling_HTTP_Methods_and_Routing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<TodoItem> ToDoItems = new List<TodoItem>();

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/todoitems", () =>
            {
                return Results.Ok(ToDoItems);
            });

            app.MapGet("/todoitems/{id}", (int id) =>
            {
                var index = ToDoItems.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                return Results.Ok(ToDoItems[index]);
            });

            app.MapPost("/todoitems", (TodoItem item) =>

            {
                ToDoItems.Add(item);

                return Results.Created();
            });

            app.MapPost("/todoitems", (TodoItem item) =>
            {
                ToDoItems.Add(item);

                return Results.Created();

            }).AddEndpointFilter(async (context, next) =>
            {
                var toDoItem = context.GetArgument<TodoItem>(0);
                if (toDoItem.Assignee == "Joe Bloggs")
                {
                    return Results.Problem("Joe Bloggs cannot be assigned todo items");
                }
                return await next(context);
            });

            app.MapPost("/todoitems", (TodoItem item) =>
            {
                ToDoItems.Add(item);

                return Results.Created();

            }).AddEndpointFilter<CreateTodoFilter>();



            app.MapPut("/todoitems", (TodoItem item) =>
            {
                var index = ToDoItems.FindIndex(x => x.Id == item.Id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                ToDoItems[index] = item;
                return Results.NoContent();
            });

            app.MapPatch("/updateTodoItemDueDate/{id}", (int id, DateTime newDueDate) =>
            {
                var index = ToDoItems.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                ToDoItems[index].DueDate = newDueDate;
                return Results.NoContent();
            });

            app.MapDelete("/todoitems/{id}", (int id) =>
            {
                var index = ToDoItems.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                ToDoItems.RemoveAt(index);
                return Results.NoContent();
            });

            app.MapDelete("/todoitems/{id:int}", (int id) =>
            {
                var index = ToDoItems.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                ToDoItems.RemoveAt(index);
                return Results.NoContent();
            });

            app.MapDelete("/todoitems/{id:int:range(1,100)}", (int id) =>

            {
                var index = ToDoItems.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                ToDoItems.RemoveAt(index);
                return Results.NoContent();
            });

            app.MapPatch("/updateTodoItemDueDate/{id}", (int id, DateTime newDueDate) =>
            {
                var index = ToDoItems.FindIndex(x => x.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                ToDoItems[index].DueDate = newDueDate;
                return Results.NoContent();
            });

            app.MapPost("/todoitems", (TodoItem item) =>
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(item);
                bool isValid = Validator.TryValidateObject(item, validationContext, validationResults, true);
                if (!isValid)
                {
                    return Results.BadRequest(validationResults);
                }
                ToDoItems.Add(item);
                return Results.Created();
            });

            app.Run();
        }
    }
}
