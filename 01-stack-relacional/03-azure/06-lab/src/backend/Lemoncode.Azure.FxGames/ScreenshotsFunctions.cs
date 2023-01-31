using System.IO;
using Microsoft.Azure.WebJobs;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static Lemoncode.Azure.Images.Helpers.ImageHelper;
using Microsoft.Extensions.Logging;
using Lemoncode.Azure.Models;
using Azure.Storage.Blobs;
using Azure.Storage;
using Lemoncode.Azure.Models.Configuration;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace Lemoncode.Azure.FxGames
{
    public class ScreenshotsFunctions
    {
        private readonly ILogger<ScreenshotsFunctions> logger;

        public ScreenshotsFunctions(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ScreenshotsFunctions>();
        }


        [FunctionName("ResizeImage")]
        public void ResizeImageFunction(
            [BlobTrigger("screenshots/{folder}/{name}", Connection = "AzureWebJobsGamesStorage")] Stream image,
            [Blob("thumbnails/{folder}/{name}", FileAccess.Write, Connection = "AzureWebJobsGamesStorage")] Stream imageSmall,
            [Queue("screenshots")] string message,
            string name,
            string folder)
        {
            logger.LogInformation($"Screenshot = {name} in folder {folder}");
            IImageFormat format;

            using (Image<Rgba32> input = SixLabors.ImageSharp.Image.Load<Rgba32>(image, out format))
            {
                ResizeImage(input, imageSmall, ImageSize.Small, format);
            }

            message = JsonConvert.SerializeObject(new ScreenshotMessage
            {
                GameId = Int32.Parse(folder),
                Filename = name,
                ScreenshotUrl = string.Empty
            });
        }

        //[FunctionName("ResizeImageQueue")]
        //public async Task ResizeImageFunctionQueue(
        //    [QueueTrigger("screenshots")] string message)
        //{
        //    var screenshotMessage = JsonConvert.DeserializeObject<ScreenshotMessage>(message);
        //    var storageConnection = Environment.GetEnvironmentVariable("AzureWebJobsGamesStorage");
        //    var blobPath = $"{screenshotMessage.GameId}/{screenshotMessage.Filename}";
        //    BlobClient blobClient = new BlobClient(
        //        storageConnection,
        //        "screenshots",
        //        blobPath);
        //    var image = await blobClient.OpenReadAsync();
        //    IImageFormat format;
        //    Stream imageSmall = new MemoryStream();

        //    using (Image<Rgba32> input = SixLabors.ImageSharp.Image.Load<Rgba32>(image, out format))
        //    {
        //        ResizeImage(input, imageSmall, ImageSize.Small, format);
        //    }
        //}

        //[FunctionName("UdpdateGameScreenshotUrl")]
        //public void UdpdateGameScreenshotUrl(
        //    [QueueTrigger("thumbnails")] string message)
        //{
        //    var screenshotMessage = JsonConvert.DeserializeObject<ScreenshotMessage>(message);

        //    SqlConnection
        //}
    }
}
