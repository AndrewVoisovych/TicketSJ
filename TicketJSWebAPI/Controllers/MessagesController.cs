using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace TicketJSWebAPI.Controllers
{

    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://softjourn.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=hpy8CVedWorWIJ7beieC4TdeRFJw03xlqo5BbSNOZtY=";
        private const string QueueName = "softjournqueue";
        static IQueueClient queueClient;
        private int numberOfMessagesToSend;
        private List<string> logsMessages;

        public MessagesController()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            logsMessages = new List<string>();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] int rangeElement)
        {
            this.numberOfMessagesToSend = (rangeElement == 0) ? 3 : rangeElement;
            try
            {
                TicketForJson[] ticket = new TicketForJson[numberOfMessagesToSend];
                DataGenerator dg;

                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    ticket[i] = new TicketForJson();
                    dg = new DataGenerator(ticket[i]);

                    logsMessages.AddRange(dg.GetLogs());

                    await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ticket[i]))));
                    logsMessages.Add((i + 1) + ". Ticket number: " + ticket[i].number + " send");
                }


                return StatusCode(200, logsMessages);
            }
            catch (Exception e)
            {
                logsMessages.Add("Unsuccessful sending:" + e.Message);
                return StatusCode(500, logsMessages);
            }
        }
    }
}