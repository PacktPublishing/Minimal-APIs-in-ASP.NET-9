namespace Chapter_6___Parameter_Binding
{
    public class TodoItemService
    {
        List<TodoItem> todoItems = new List<TodoItem>();
        public TodoItem GetById(int id)
        {
            return todoItems.FirstOrDefault(x => x.Id == id);
        }

        public List<TodoItem> GetTodoItems(bool pastDue, int priority)
        {
            var todoItemsQuery = todoItems.AsQueryable();
            if (pastDue)
            {
                todoItemsQuery = todoItemsQuery.Where(x => x.DueDate <= DateTime.Now);
            }
            if (priority > 0)
            {
                todoItemsQuery = todoItemsQuery.Where(x => x.Priority == priority);
            }
            return todoItemsQuery.ToList();
        }
    }
}
