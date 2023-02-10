using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Contracts;
using ToDoListApp.DBContext;
using ToDoListApp.Models;
using ToDoListApp.Services;

namespace ToDoListApp.Repository
{
    //Repository handling task actions
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskToDoContext _dbContext;
        private readonly IRedisCacheService _redisCache;
        private readonly ILogService _logger;
        private enum actionType { Create, Update, Delete};

        public TaskRepository(TaskToDoContext dbContext, IRedisCacheService redisCache, ILogService logger)
        {
            _dbContext = dbContext;
            _redisCache = redisCache;
            _logger = logger;
        }

        /// <summary>
        /// Create tasks - adding them to the database using Entity Framework. 
        /// </summary>
        /// <param name="newTasks">new tasks to be created</param>
        /// <returns></returns>
        public async Task<TasksToDoStatusResponse> CreateTask(TaskToDo[] newTasks)
        {
            if(newTasks != null && newTasks.Length >0)
            {
                try
                {
                    //Adding entities using the AddRange method improves the performance.
                    //It is recommended to use this method when inserting large number of records from the database using Entity Framework.
                    _dbContext.TasksToDo.AddRange(newTasks);
                    int rowsAdded = await _dbContext.SaveChangesAsync();
                    string msg = $"{rowsAdded} tasks added successfuly from {newTasks.Length} total tasks.";
                    CheckNumberOfTasksAffected(rowsAdded, newTasks, msg, actionType.Create);

                    return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.OK, Msg= msg };
                }
                catch(Exception ex)
                {
                    _logger.WriteErrorLogForAction("*********Create Tasks", ex.Message, $"Tasks:{GetTasksDescription(newTasks)}\n*********");

                    return new TasksToDoStatusResponse()
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        ErrorMsg = $"Create Task Exception: {ex.Message}"
                    };
                }
                
            }
           
            return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.BadRequest, Msg = "Please provide tasks" };
        }

        /// <summary>
        /// Delete tasks - deleting them from the database using Entity Framework.
        /// </summary>
        /// <param name="tasksToDelete">tasks to be deleted</param>
        /// <returns></returns>
        public async Task<TasksToDoStatusResponse> DeleteTask(TaskToDo[] tasksToDelete)
        {
            if (tasksToDelete != null && tasksToDelete.Length > 0)
            {
                try
                {
                    //Removing entities using the RemoveRange method improves the performance.
                    //It is recommended to use this method when deleting large number of records from the database using Entity Framework.
                    _dbContext.RemoveRange(tasksToDelete);
                    int rowsDeleted = await _dbContext.SaveChangesAsync();
                    string msg = $"{rowsDeleted} tasks deleted successfuly from {tasksToDelete.Length} total tasks.";
                    CheckNumberOfTasksAffected(rowsDeleted, tasksToDelete, msg, actionType.Delete);

                    return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.OK, Msg = msg };
                }
                catch(Exception ex)
                {
                    _logger.WriteErrorLogForAction("***********Delete Tasks", ex.Message, $"Tasks:{GetTasksDescription(tasksToDelete)}\n*********");
                    
                    return new TasksToDoStatusResponse() 
                    { 
                        StatusCode = (int)HttpStatusCode.InternalServerError, 
                        ErrorMsg = $"Delete Task Exception: {ex.Message}" 
                    };
                }
            }

            return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.BadRequest, Msg="Please provide tasks to delete" };
        }

        /// <summary>
        /// Update tasks - updating them in database using Entity Framework.
        /// </summary>
        /// <param name="tasksModified"></param>
        /// <returns></returns>
        public async Task<TasksToDoStatusResponse> UpdateTask(TaskToDo[] tasksModified)
        {
            if (tasksModified != null && tasksModified.Length > 0)
            {
                try
                {
                    _dbContext.UpdateRange(tasksModified);
                    int rowsUpdated = await _dbContext.SaveChangesAsync();
                    string msg = $"{rowsUpdated} tasks updated successfuly from {tasksModified.Length} total tasks.";
                    CheckNumberOfTasksAffected(rowsUpdated, tasksModified, msg, actionType.Update);

                    return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.OK, Msg=msg };
                }
                catch(Exception ex)
                {
                    _logger.WriteErrorLogForAction("**********Update Tasks", ex.Message, $"Tasks change:{GetTasksDescription(tasksModified)}\n*********");
                    return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.InternalServerError,  ErrorMsg = "Update Task Exception: "+ex.Message};
                }
                
            } 

            return new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.BadRequest, Msg = "Please provide tasks to update" };
        }

        /// <summary>
        /// Get tasks - support pagination for reading a list of tasks.
        /// </summary>
        /// <param name="tasksParameters"></param>
        /// <returns></returns>
        public TaskToDoListResponse GetTasks(TasksParameters tasksParameters)
        {
            return GetTasksByPaging((tasksParameters.PageNumber - 1) * tasksParameters.PageSize, tasksParameters.PageSize);
        }

        /// <summary>
        /// Get tasks by paging using skip and take parameters. 
        /// It uses Redis cache. If tasks are not in cache it retrieves them from database and update cache.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        private TaskToDoListResponse GetTasksByPaging(int skip, int take)
        {
            List<TaskToDo> list = new List<TaskToDo>();

            try
            {
                if(take>0 && skip>=0)
                {
                    var cachedTasksByPaging = _redisCache.Get<List<TaskToDo>>($"tasksSkip{skip}take{take}");
                    string msgRetrieved = "data retrieved successfully";

                    if (cachedTasksByPaging != null)// return cache saved tasks (for req page number and page size)
                        return new TaskToDoListResponse() { 
                            Tasks = cachedTasksByPaging, 
                            TasksToDoStatus = new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.OK, Msg = msgRetrieved } 
                        };
                    else
                    {
                        //not in cache- retrieve from DB:
                        //It is recommended to do pagination in DB (ant not in code) to improve performance, therefore call sp GetTasksByPaging
                        var param1 = new SqlParameter("@Skip", skip);
                        var param2 = new SqlParameter("@Take", take);
                        list = _dbContext.TasksToDo.FromSqlRaw("GetTasksByPaging {0}, {1}", new SqlParameter[] { param1, param2 }).ToList<TaskToDo>();
                        
                        if (list != null)
                            _redisCache.Set<List<TaskToDo>>($"tasksSkip{skip}take{take}", list);//lets now cache these results.

                        return new TaskToDoListResponse()
                        {
                            Tasks = list,
                            TasksToDoStatus = new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.OK, Msg = msgRetrieved }
                        };
                    } 
                }
                
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                _logger.WriteErrorLogForAction("GetTasksByPaging", ex.Message, $"Skip: {skip} Take: {take}");

                return new TaskToDoListResponse()
                {
                    Tasks = list,
                    TasksToDoStatus = new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.InternalServerError, ErrorMsg = msg }
                };
            }

            return new TaskToDoListResponse()
            {
                Tasks = list,
                TasksToDoStatus = new TasksToDoStatusResponse() { StatusCode = (int)HttpStatusCode.BadRequest, Msg= "Please provide valid pageNumber and pageSize" }
            };
            
        }

        /// <summary>
        /// Get tasks description for relevant tasks
        /// </summary>
        /// <param name="tasks">relevant tasks</param>
        /// <returns></returns>
        private string GetTasksDescription(TaskToDo[] tasks)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TaskToDo task in tasks)
            {
                sb.AppendLine(task.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compare rows affected with tasks that should be affected (tasks parameter contains the tasks with the changes)
        /// </summary>
        /// <param name="rowsAffected"></param>
        /// <param name="tasks"></param>
        /// <param name="msg"></param>
        private void CheckNumberOfTasksAffected(int rowsAffected, TaskToDo[] tasks, string msg, actionType action)
        {
            if (rowsAffected != tasks.Length)
            {
                if (rowsAffected == 0)
                {
                    if(action == actionType.Update)
                        msg = msg + $"\n All tasks were not updated: {GetTasksDescription(tasks)}\n*********";
                    else if(action == actionType.Create)
                        msg = msg + $"\n All tasks were not added: {GetTasksDescription(tasks)}\n*********";
                    else if (action == actionType.Delete)
                        msg = msg + $"\n All tasks were not deleted: {GetTasksDescription(tasks)}\n*********";
                }

                _logger.WriteInformationLog(msg);
            }
        }
    }
}
