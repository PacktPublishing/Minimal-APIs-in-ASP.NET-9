using System.ComponentModel.DataAnnotations;

namespace Chapter_4___Handling_HTTP_Methods_and_Routing
{
    public class TodoItem

    {

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
        
        [Required(ErrorMessage = "You need to add a title my dude!")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Assignee { get; set; }

        public int Priority { get; set; }

        public bool IsComplete { get; set; }

    }
}
