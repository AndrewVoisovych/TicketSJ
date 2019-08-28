using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using WindowsService.Utils.Helpers;
using WindowsService.Models;


namespace WindowsService.AzureReceive
{
    /// <summary>
    /// The class that reads out the Azure ServiceBus queue and calls for adding to Azure Sql as well as logging data
    /// </summary>
    public sealed class ReceiveMessages
    {
        /// <summary>
        /// Class Configuration Helper
        /// </summary>
        private ConfigurationHelper configuration;

        // Azure ServiceBusConnectionString
        private readonly string ServiceBusConnectionString;
        private readonly string QueueName;

        /// <summary>
        /// QueueClient can be used for all basic interactions with a Service Bus Queue.
        /// </summary>
        static IQueueClient queueClient;

        /// <summary>
        /// Write Logs Data Command
        /// </summary>
        static WrittenFileHelper file; 
       
        public ReceiveMessages()
        {

            configuration = new ConfigurationHelper();

            //get ServiceBusConnectionString from appsettings.json
            ServiceBusConnectionString = configuration.GetServiceBusConnectionString();

            //get QueueName from appsettings.json
            QueueName = configuration.GetQueueNameString();

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            file = new WrittenFileHelper();


            RegisterOnMessageHandlerAndReceiveMessages();    
        }

        /// <summary>
        /// Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
        /// </summary>
        static void RegisterOnMessageHandlerAndReceiveMessages()
        { 
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message and write to log
           file.Write($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
          

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            //Deserialize Object to class
            Ticket ticket = JsonConvert.DeserializeObject<Ticket>(Encoding.UTF8.GetString(message.Body));
            
            //Add to Database
            AddDatabase objectToDatabase = new AddDatabase();

            // Write to log
            file.Write($"{objectToDatabase.Add(ticket)}{Environment.NewLine}" );

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {    
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            // -- Show Errors Message and other details
            //Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            //Console.WriteLine("Exception context for troubleshooting:");
            //Console.WriteLine($"- Endpoint: {context.Endpoint}");
            //Console.WriteLine($"- Entity Path: {context.EntityPath}");
            //Console.WriteLine($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}
