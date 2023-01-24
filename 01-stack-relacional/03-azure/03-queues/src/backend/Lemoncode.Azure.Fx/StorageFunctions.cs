using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Lemoncode.Azure.Fx
{
    public class StorageFunctions
    {
        private readonly ILogger logger;

        public StorageFunctions(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<StorageFunctions>();
        }
        [Function("MoveFile")]
        [BlobOutput("fxout/{name}.txt")]
        public static string MoveFiles(
            [BlobTrigger("fxin/{name}.txt", Connection = "AzureWebJobsStorage")] string myTriggerBlob,
            FunctionContext context,
            string name)
        {
            var logger = context.GetLogger("BlobFunction");
            logger.LogInformation($"Triggered Item = {myTriggerBlob}");

            // Blob Output
            return myTriggerBlob;
        }

        [Function("SaveQueueMessage")]
        [QueueOutput("testqueue")]
        public static string SaveQueueMessage(
            [BlobTrigger("fxin/{name}.gif", Connection = "AzureWebJobsStorage")] string myTriggerBlob,
            FunctionContext context,
            string name)
        {
            var logger = context.GetLogger("BlobFunction");
            logger.LogInformation($"Triggered Item = {myTriggerBlob}");

            return "Metiendo un mensaje en la cola";
        }
    }
}
