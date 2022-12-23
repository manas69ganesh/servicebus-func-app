using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Company.Function
{
    public class TestServiceBusTopicTrigger
    {
        private readonly ILogger<TestServiceBusTopicTrigger> _logger;

        public TestServiceBusTopicTrigger(ILogger<TestServiceBusTopicTrigger> log)
        {
            _logger = log;
        }

        [FunctionName("TestServiceBusTopicTrigger")]
        public void Run([ServiceBusTrigger("nso-topic1", "nso-subscription2", Connection = "SERVICEBUS_CONN_STRING")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger  function processed message: {mySbMsg}");
            // write code to insert into cosmos db
            // New instance of CosmosClient class
            var client = new MongoClient(Environment.GetEnvironmentVariable("COSMOSDB_CONN_STRING"));
            // Database reference with creation if it does not already exist
            var db = client.GetDatabase("nso-cloudmes-db");
            // Container reference with creation if it does not alredy exist
            var collection = db.GetCollection<Entity>("nso-cloudmes-topic-publish");

            Entity entity = JsonConvert.DeserializeObject<Entity>(mySbMsg);

            collection.InsertOne(entity);


        }
    }
}
