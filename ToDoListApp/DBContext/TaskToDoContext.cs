using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.DBContext
{
    public class TaskToDoContext: DbContext
    {
        public TaskToDoContext(DbContextOptions<TaskToDoContext> options) : base(options)
        {
        }

        public DbSet<TaskToDo> TasksToDo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
