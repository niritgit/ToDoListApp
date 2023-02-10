using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApp.Services
{
    public interface ILogService
    {
        void WriteErrorLogForAction(string actionType, string exception, string additionalDetails);

        void WriteErrorLog(string exception, string additionalDetails);

        void WriteInformationLog(string msg);
        
    }
}
