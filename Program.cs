using Chapter_6___Parameter_Binding;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;




List<TodoItem> TodoItems = new List<TodoItem>();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<TodoItemService>();
var app = builder.Build();

app.MapGet("/todoitems/{id}", (int id) =>
{
    TodoItem item = GetById(id);
    if(item != null)
    {
        return Results.Ok(item);
    }
    return Results.NotFound();
   
});

app.MapGet("/todoitems/{id}", (HttpRequest request) =>
{

    if(int.TryParse(request.RouteValues["id"].ToString(), out var id) == false)
{
        return Results.BadRequest("Could not convert id to integer");
    }
    TodoItem item = GetById(id);
    return Results.Ok(item);
});

app.MapGet("/todoitems/{id}", (int id, string? assignee) =>
{

    var index = TodoItems.FindIndex(x => x.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }
    var todoItem = TodoItems[index];
    if (assignee != null)
    {
        if (todoItem.Assignee != assignee)
        {
            return Results.NotFound();
        }
    }
    return Results.Ok(TodoItems[index]);
});

app.MapGet("/todoItems", (HttpRequest request) =>
{
    bool pastDue = false;
    int priority = 0;

    var todoItemsQuery = TodoItems.AsQueryable();

    if (request.Query.ContainsKey("pastDue"))
    {
        var parsedDueDate = bool.TryParse(request.Query["pastDue"], out pastDue);
        if (parsedDueDate) { todoItemsQuery = todoItemsQuery.Where(x => x.DueDate <= DateTime.Now); }
    }
    if (request.Query.ContainsKey("priority"))
    {
        var parsedPriority = int.TryParse(request.Query["priority"], out priority);
        if (parsedPriority) { todoItemsQuery = todoItemsQuery.Where(x => x.Priority == priority); }
    }

    var result = todoItemsQuery.ToList();
    return Results.Ok(result);
});

app.MapGet("/todoItems", (HttpRequest request) =>
{
    var customHeader = request.Headers["SomeCustomHeader"];
    var result = TodoItems.ToList();
    return Results.Ok(result);
});

app.MapPost("/todoitems", (TodoItem item) =>
{
    var validationContext = new ValidationContext(item);
    var validationResults = new List<ValidationResult>();
    var isValid = Validator.TryValidateObject(item, validationContext, validationResults, true);
    if (isValid)
    {
        TodoItems.Add(item);
        return Results.Created();
    }
    return Results.BadRequest(validationResults);
});

app.MapPatch("/updateTodoItemDueDate", (HttpRequest request) =>
{
    var id = int.Parse(request.Form["Id"]);
    var newDueDate = DateTime.Parse(request.Form["newDueDate"]);

    var index = TodoItems.FindIndex(x => x.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }
    TodoItems[index].DueDate = newDueDate;
    return Results.NoContent();
});

app.MapGet("/todoItems", ([FromQuery(Name = "pastDue")] bool pastDue,
                          [FromQuery(Name = "priority")] int priority) =>
{
    var todoItemsQuery = TodoItems.AsQueryable();
    if (pastDue)
    {
        todoItemsQuery = todoItemsQuery.Where(x => x.DueDate <= DateTime.Now);
    }
    if (priority > 0)
    {
        todoItemsQuery = todoItemsQuery.Where(x => x.Priority == priority);
    }
    var result = todoItemsQuery.ToList();
    return Results.Ok(result);
});

app.MapPost("/todoitems", (TodoItem item,
                        [FromHeader(Name = "TriggerBackgroundTask")] bool triggerBackgroundTaskHeader) =>
{
    if (triggerBackgroundTaskHeader)
    {
        // do something else in the background
    }

    TodoItems.Add(item);
    return Results.Created();
});

app.MapGet("/todoItems", ([FromQuery(Name = "pastDue")] bool pastDue,
                          [FromQuery(Name = "priority")] int priority,
                          [FromServices] TodoItemService todoItemService) =>
{
   
    var result = todoItemService.GetTodoItems(pastDue, priority);
    return Results.Ok(result);
});








TodoItem GetById(int id)
{
     return TodoItems.FirstOrDefault(x => x.Id == id);
}


app.Run();
