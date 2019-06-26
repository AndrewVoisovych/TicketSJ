using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TicketJSWebAPI
{
    public class ConfigurationHelper
    {
        private  string connection;
        private  string queueName;

        public  string ServiceBusConnectionString()
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = GetServiceBusConnectionString();
            }

            return connection;
        }

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
