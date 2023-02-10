using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToDoListApp.Models
{
    /// <summary>
    /// Task to do class. 
    /// Each task has id (key), title, description, status, due date, created at and updated at.
    /// </summary>
    public class TaskToDo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "A task title is required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "For title use letters or digits only please")]
        public string Title { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s_.-]+$", ErrorMessage = "For description use valid characters please")]
        public string Description { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "For status use letters only please")]
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Id:{Id} ");
            sb.Append($"Title:{Title} ");
            sb.Append($"Description:{Description} ");
            sb.Append($"Status:{Status} ");
            sb.Append($"DueDate:{DueDate} ");
            sb.Append($"CreatedAt:{CreatedAt} ");
            sb.Append($"UpdatedAt:{UpdatedAt} ");
            return sb.ToString();
        }
    }
}
