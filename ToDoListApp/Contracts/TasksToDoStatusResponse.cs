using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Contracts
{
    /// <summary>
    /// This class contains HTTP status code and suitable msg/error responses.
    /// It is in use for an action method response.
    /// </summary>
    public class TasksToDoStatusResponse
    {
        public int StatusCode { get; set; }

        public string Msg { get; set; }

        public string ErrorMsg { get; set; }
    }
}
