using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Contracts
{
    public class TaskToDoListResponse
    {
        public TasksToDoStatusResponse TasksToDoStatus { get; set; }

        public List<TaskToDo> Tasks { get; set; }
    }
}
