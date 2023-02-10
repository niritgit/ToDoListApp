using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Contracts
{
    /// <summary>
    /// This class contains HTTP status code and suitable msg/error responses and also list of relevant to do tasks.
    /// It is in use for an action method response.
    /// </summary>
    public class TaskToDoListResponse
    {
        public TasksToDoStatusResponse TasksToDoStatus { get; set; }

        public List<TaskToDo> Tasks { get; set; }
    }
}
