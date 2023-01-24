using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Lemoncode.Azure.Fx
{
    public class QueueFunctions
    {
        private readonly ILogger logger;

        public QueueFunctions(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<QueueFunctions>();
        }

        [Function("QueueFunctions")]
        public void Run(
            [QueueTrigger("testqueue", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
