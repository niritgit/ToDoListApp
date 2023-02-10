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

        [HttpPost]
        [Route("CreateTask")]
        public async Task<IActionResult> CreateTask(TaskToDo[] newTasks)
        {
            var res = await _tasksRepository.CreateTask(newTasks);
            return GetActionResult(res);
        }

        [HttpPut]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask(TaskToDo[] modifiedTasks)
        {
            var res = await _tasksRepository.UpdateTask(modifiedTasks);
            return GetActionResult(res);
        }

        [HttpDelete]
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask(TaskToDo[] tasks)
        {
            var res = await _tasksRepository.DeleteTask(tasks);
            return GetActionResult(res);
        }

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
