using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApp.Contracts
{
    /// <summary>
    /// This class is for pagination - it has page size and page number.
    /// </summary>
    public class TasksParameters
    {
        const int maxPageSize = 50;
        [Range(1, Int32.MaxValue, ErrorMessage = "Page number must be >=1")]
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 2;
        [Range(1, Int32.MaxValue, ErrorMessage = "Page size must be >=1")]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
