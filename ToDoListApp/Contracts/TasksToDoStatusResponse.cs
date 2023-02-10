using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Contracts
{
    public class TasksToDoStatusResponse
    {
        public int StatusCode { get; set; }

        public string Msg { get; set; }

        public string ErrorMsg { get; set; }
    }
}
