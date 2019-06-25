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

        public MessagesController()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            numberOfMessagesToSend = 10;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                TicketForJson[] ticket = new TicketForJson[numberOfMessagesToSend];
                DataGenerator dg;

                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    ticket[i] = new TicketForJson();
                    dg = new DataGenerator(ticket[i]);

                    await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ticket[i]))));
                }


                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var objJson = new TicketForJson()
                {
                    number = 1,
                    description = "first ticket test",
                    type = TicketType.free,
                    dateTime = DateTime.Now,
                    tags = new string[] { "tags1", "tags2", "tags3" }
                };


                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objJson))));
                }


                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

    }
}