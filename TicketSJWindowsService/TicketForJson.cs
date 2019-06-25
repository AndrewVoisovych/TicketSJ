using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSJWindowsService
{
    public enum TicketType { free, paid, preferential, special};
    public class TicketForJson
    {
        public int number { get; set; }
        public string description { get; set; }
        public DateTime dateTime { get; set; }
        public TicketType type { get; set; }
        public string[] tags { get; set; }
    }

}
