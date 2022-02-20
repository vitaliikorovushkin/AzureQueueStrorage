using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureQueueStorage1
{
    class Program
    {
        static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("config1.json")
               .Build();

            string connectionString = configuration["StorageConnectionString"];
            //await CreateQueue(connectionString, "myqueue");
            //await AddMessageIntoQueue(connectionString, "myqueue", "My first message in queue");
            //await AddMessageIntoQueue(connectionString, "myqueue", "My second message in queue");
            //Console.WriteLine(await PeekNextMessage(connectionString, "myqueue"));
            //Console.WriteLine(await GetNextMessage(connectionString, "myqueue"));
            await DeleteMessage(connectionString, "myqueue");
        }

        public static async Task CreateQueue(string connectionString, string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
        
        
        }

        public static async Task AddMessageIntoQueue(string connectionString, string queueName, string msg)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            CloudQueueMessage message = new CloudQueueMessage(msg);
            //message.Add
            await queue.AddMessageAsync(message, TimeSpan.MaxValue, null, null, null);

        }

        public static async Task<String> PeekNextMessage(string connectionString, string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            CloudQueueMessage peekedMessage = await queue.PeekMessageAsync();
            return peekedMessage.AsString;
        }

        public static async Task<String> GetNextMessage(string connectionString, string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            await queue.FetchAttributesAsync();
            Console.WriteLine("Before: " + queue.ApproximateMessageCount);
            CloudQueueMessage message = await queue.GetMessageAsync();
            Thread.Sleep(1000);
            await queue.FetchAttributesAsync();
            Console.WriteLine("After: " + queue.ApproximateMessageCount);
            return message.AsString;
        }

        public static async Task DeleteMessage(string connectionString, string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();
            await queue.DeleteMessageAsync(retrievedMessage);

        }
    }
}
