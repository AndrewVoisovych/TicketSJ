using System.Configuration;

namespace WindowsService.Utils.Helpers
{

    /// <summary>
    /// Helper for obtaining connection data to Azure Service Bus and Azure SQL
    /// </summary>
    public sealed class ConfigurationHelper
    {
        /// <summary>
        /// Get Service Bus conenction string
        /// </summary> 
        public string GetServiceBusConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"].ConnectionString.ToString();
        }

        /// <summary>
        /// Get Queue Name
        /// </summary>
        public string GetQueueNameString()
        {
            return ConfigurationManager.AppSettings["QueueName"].ToString();
        }

        /// <summary>
        /// Get SQL conenction string
        /// </summary>
        public string GetDatabaseConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["TicketSoftjournContext"].ConnectionString.ToString();
        }

    }
}
