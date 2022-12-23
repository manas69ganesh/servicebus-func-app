using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;


namespace Company.Function
{
    public class TestServiceBusQueueTrigger
    {
        [FunctionName("TestServiceBusQueueTrigger")]
        public void Run([ServiceBusTrigger("nso-queue1", Connection = "SERVICEBUS_CONN_STRING")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function  processed message: {myQueueItem}");
            // write code to insert into cosmos db
            // New instance of CosmosClient class
            var client = new MongoClient(Environment.GetEnvironmentVariable("COSMOSDB_CONN_STRING"));
            // Database reference with creation if it does not already exist
            var db = client.GetDatabase("nso-cloudmes-db");
            // Container reference with creation if it does not alredy exist
            var collection = db.GetCollection<Entity>("nso-cloudmes-queue-publish");

            Entity entity = JsonConvert.DeserializeObject<Entity>(myQueueItem);

            collection.InsertOne(entity);
        }
    }
}
