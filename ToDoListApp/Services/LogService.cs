using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApp.Services
{
    public class LogService: ILogService
    {
        private readonly ILogger _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }
        public void WriteErrorLogForAction(string actionType, string exception, string additionalDetails)
        {
            _logger.LogError($"{actionType} failed :\n Exception: {exception} \n {additionalDetails}");
        }

        public void WriteErrorLog(string exception, string additionalDetails)
        {
            _logger.LogError($"Exception: {exception} \n {additionalDetails}");
        }

        public void WriteInformationLog(string msg)
        {
            _logger.LogInformation(msg);
        }
    }
}
