using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Contracts;
using ToDoListApp.Models;

namespace ToDoListApp.Repository
{
    //Interface for task actions
    public interface ITaskRepository
    {
        TaskToDoListResponse GetTasks(TasksParameters tasksParameters);
        Task<TasksToDoStatusResponse> CreateTask(TaskToDo[] newTasks);
        Task<TasksToDoStatusResponse> UpdateTask(TaskToDo[] tasksModified);
        Task<TasksToDoStatusResponse> DeleteTask(TaskToDo[] tasksToDelete);
    }
}
