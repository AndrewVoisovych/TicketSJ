using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketJSWebAPI
{
    public class ServiceBusHelper
    {
        public static QueueClient GetQueueClient(ReceiveMode receiveMode = ReceiveMode.ReceiveAndDelete)
        {
            const string queueName = "softjournqueue";
            var queueClient = new QueueClient(ConfigurationHelper.ServiceBusConnectionString(), queueName, receiveMode, GetRetryPolicy());
            return queueClient;
        }

        public static TopicClient GetTopicClient(string topicName = "stockupdated")
        {
            var topicClient = new TopicClient(ConfigurationHelper.ServiceBusConnectionString(), topicName, GetRetryPolicy());
            return topicClient;
        }

        private static RetryExponential GetRetryPolicy()
        {
            return new RetryExponential(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 10);
        }
    }
}
