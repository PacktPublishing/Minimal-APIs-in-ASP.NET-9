
namespace Chapter_4___Handling_HTTP_Methods_and_Routing
{
    public class CreateTodoFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var todoItem = context.GetArgument<TodoItem>(0);
            if(todoItem.Assignee == "Joe Bloggs")
            {
                return Results.Problem("Joe Bloggs cannot be assigned a todoitem");
            }
            return await next(context);
        }
    }
}
