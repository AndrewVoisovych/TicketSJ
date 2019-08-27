using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using WebAPI.Models;
using WebAPI.Utils.Helpers;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    public sealed class MessagesController : Controller
    {
        // Azure ServiceBusConnectionString
        /// <summary>
        /// Class Configuration Helper
        /// </summary>
        private ConfigurationHelper configuration; 
        private readonly string ServiceBusConnectionString;
        private readonly string QueueName;

        /// <summary>
        /// QueueClient can be used for all basic interactions with a Service Bus Queue
        /// </summary>
        static IQueueClient queueClient;

        /// <summary>
        /// Number of tickets to generate
        /// </summary>
        private int numberOfMessagesToSend;

        /// <summary>
        /// Information for print on html
        /// </summary>
        private List<string> logsMessages;

        public MessagesController()
        {
            configuration = new ConfigurationHelper();

            //get ServiceBusConnectionString from appsettings.json
            ServiceBusConnectionString = configuration.ServiceBusConnectionString();
            
            //get QueueName from appsettings.json
            QueueName = configuration.QueueNameString();

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            logsMessages = new List<string>();

        }

        /// <summary>
        /// Create, fill and send tickets async
        /// </summary>
        /// <param name="rangeElement">Number of tickets to generate</param>
        /// <returns>Status Code OK 200 or StatusCode Error 500</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] int rangeElement)
        {
            //Number of tickets to generate. Default: 1
           numberOfMessagesToSend = (rangeElement == 0) ? 1 : rangeElement;

            try
            {
                Ticket[] ticket = new Ticket[numberOfMessagesToSend];

                DataGenerator generatedData;

                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Generate Ticket
                    ticket[i] = new Ticket();
                    
                    // Generate Data
                    generatedData = new DataGenerator(ticket[i]); 


                    logsMessages.AddRange(generatedData.GetLogs());
                    
                    //Send
                    await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ticket[i]))));

                    logsMessages.Add($"{i+1}. Ticket number: {ticket[i].Number} send");        
                  
                }

                //return Status Code OK and Information for print on html
                return StatusCode(200, logsMessages);
               
            }
            catch (Exception e)
            {
                logsMessages.Add($"Unsuccessful sending: {e.Message}");
                
                //return Status Code Internal Server Error and Information for print on html
                return StatusCode(500, logsMessages);
            }
        }
    }
}