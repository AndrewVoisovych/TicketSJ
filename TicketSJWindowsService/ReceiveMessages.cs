using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace TicketSJWindowsService
{

    //The class that reads out the Azure ServiceBus queue and calls for adding to Azure Sql as well as logging data
    class ReceiveMessages
    {
        // Azure ServiceBusConnectionString
        const string ServiceBusConnectionString = "Endpoint=sb://softjourn.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=hpy8CVedWorWIJ7beieC4TdeRFJw03xlqo5BbSNOZtY=";
        const string QueueName = "softjournqueue";

        
        static IQueueClient queueClient; //QueueClient can be used for all basic interactions with a Service Bus Queue.
        static WriteToFile writeCommand; //Write Logs Data Command
       
        public ReceiveMessages()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            writeCommand = new WriteToFile();
            RegisterOnMessageHandlerAndReceiveMessages();    
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
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
           writeCommand.Write($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
          

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            //Deserialize Object to class
            TicketForJson ticket = JsonConvert.DeserializeObject<TicketForJson>(Encoding.UTF8.GetString(message.Body));
            
            //Add to Database
            AddingToDatabase objectToDatabase = new AddingToDatabase();

            // Write to log
            writeCommand.Write(objectToDatabase.Add(ticket)+"\n");

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            //Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            //Console.WriteLine("Exception context for troubleshooting:");
            //Console.WriteLine($"- Endpoint: {context.Endpoint}");
            //Console.WriteLine($"- Entity Path: {context.EntityPath}");
            //Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
