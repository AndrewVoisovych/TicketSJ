using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebAPI.Utils.Helpers
{
    /// <summary>
    /// Helper for obtaining connection data to Azure Service Bus
    /// </summary>
    public sealed class ConfigurationHelper
    {
        private string connection;
        private string queueName;

        /// <summary>
        /// Get Service Bus conenction string
        /// </summary>
        /// <returns></returns>
        public string ServiceBusConnectionString()
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = GetServiceBusConnectionString();
            }


            return connection;
        }

        /// <summary>
        /// Get Queue Name
        /// </summary>
        /// <returns></returns>
        public  string QueueNameString()
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                queueName = GetQueueNameString();
            }


            return queueName;
        }

        private  string GetServiceBusConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var value = config.GetValue<string>("ServiceBusConnectionString");


            return value;
        }

        private  string GetQueueNameString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var value = config.GetValue<string>("QueueName");

            return value;
        }
    }
}
