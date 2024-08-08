using System;
using System.Text.Json;
using System.Threading;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;


Console.WriteLine("Hello to the QueueProcessor!");            

var queueClient = new QueueClient(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"), "alteregos");

queueClient.CreateIfNotExists();

while (true)
{
   QueueMessage message = queueClient.ReceiveMessage();

    if (message != null)
                {
                    Console.WriteLine($"Message received {message.Body}");

                    var task = JsonSerializer.Deserialize<Task>(message.Body);

                    Console.WriteLine($"Let's rename {task.oldName} to {task.newName}");

                    if (task.oldName != null)
                    {
                        //Create a Blob service client
                        var blobClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"));

                        //Get container client
                        BlobContainerClient container = blobClient.GetBlobContainerClient("alteregos");

                        //Get blob with old name
                        var oldFileName = $"{task.oldName.Replace(' ', '-').ToLower()}.png";
                        Console.WriteLine($"Looking for {oldFileName}");
                        var oldBlob = container.GetBlobClient(oldFileName);                        

                        if (oldBlob.Exists())
                        {
                            Console.WriteLine("Found it!");
                            var newFileName = $"{task.newName.Replace(' ', '-').ToLower()}.png";
                            Console.WriteLine($"Renaming {oldFileName} to {newFileName}");

                            //Create a new blob with the new name                            
                            BlobClient newBlob = container.GetBlobClient(newFileName);

                            //Copy the content of the old blob into the new blob
                            newBlob.StartCopyFromUri(oldBlob.Uri);

                            //Delete the old blob
                            oldBlob.DeleteIfExists();

                            //Delete message from the queue
                            queueClient.DeleteMessage(message.MessageId,message.PopReceipt);
                        }
                        else
                        {
                            Console.WriteLine($"There is no old image to rename.");
                            Console.WriteLine($"Dismiss task.");
                            //Delete message from the queue
                            queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Bad message. Delete it");
                        //Delete message from the queue
                        queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                        
                    }
                }
                else
                {
                    Console.WriteLine($"Let's wait 5 seconds");
                    Thread.Sleep(5000);
                }

            }

class Task
{
    public string oldName { get; set; }
    public string newName { get; set; }
}
