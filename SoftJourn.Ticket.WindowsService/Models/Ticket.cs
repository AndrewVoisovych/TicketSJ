using System;
using Newtonsoft.Json;

namespace WindowsService.Models
{
    /// <summary>
    /// Ticket model for serialization / deserialization Json relatively Entity sample
    /// </summary>
    public sealed class Ticket
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        
        public TicketTypeEnum.TicketType Type { get; set; }

        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }
    }

}
