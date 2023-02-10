using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ToDoListApp.Models;
using ToDoListApp.Repository;

namespace ToDoListApp.Contracts
{
    /// <summary>
    /// Controller for tasks methods, handling HTTP requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _tasksRepository;
        public TasksController(ITaskRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        /// <summary>
        /// Get tasks (support pagination for reading a list of tasks)
        /// </summary>
        /// <param name="tasksParameters">taskParameters has pagination data</param>
        /// <returns></returns>
        [HttpGet(Name = "GetTasks")]
        public IActionResult GetTasks([FromQuery] TasksParameters tasksParameters)
        {
            var res = _tasksRepository.GetTasks(tasksParameters);
            return GetTasksActionResult(res);
        }

        /// <summary>
        /// Create tasks - add new tasks to database.
        /// </summary>
        /// <param name="newTasks">new tasks</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTask")]
        public async Task<IActionResult> CreateTask(TaskToDo[] newTasks)
        {
            var res = await _tasksRepository.CreateTask(newTasks);
            return GetActionResult(res);
        }

        /// <summary>
        /// Update tasks - update tasks in database.
        /// </summary>
        /// <param name="modifiedTasks">tasks with changes</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask(TaskToDo[] modifiedTasks)
        {
            var res = await _tasksRepository.UpdateTask(modifiedTasks);
            return GetActionResult(res);
        }

        /// <summary>
        /// Delete tasks - delete tasks from database.
        /// </summary>
        /// <param name="tasks">tasks to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask(TaskToDo[] tasks)
        {
            var res = await _tasksRepository.DeleteTask(tasks);
            return GetActionResult(res);
        }

        /// <summary>
        /// Get action result contains HTTP status code and suitable msg/error responses.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private IActionResult GetActionResult(TasksToDoStatusResponse res)
        {
            if (res.StatusCode != (int)HttpStatusCode.OK)
            {
                Response.StatusCode = res.StatusCode;
                //when there is an error - return relevant error message.
                return StatusCode(res.StatusCode, res);
            }
            else
            {
                return new OkObjectResult(res);
            }
        }

        /// <summary>
        /// Get action result contains HTTP status code and suitable msg/error responses and also list of relevant tasks.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private IActionResult GetTasksActionResult(TaskToDoListResponse res)
        {
            if (res != null && res.TasksToDoStatus != null
                && res.TasksToDoStatus.StatusCode != (int)HttpStatusCode.OK)
            {
                Response.StatusCode = res.TasksToDoStatus.StatusCode;
                //when there is an error - return relevant error message.
                return StatusCode(res.TasksToDoStatus.StatusCode, res);
            }
            else
            {
                return new OkObjectResult(res);
            }
        }
    }
}
