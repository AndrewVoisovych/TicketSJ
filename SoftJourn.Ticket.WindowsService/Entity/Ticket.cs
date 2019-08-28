using System;
using System.Collections.Generic;

namespace WindowsService.Entity
{
    public  partial class Ticket
    {
        public Ticket()
        {
            TicketToTags = new HashSet<TicketToTags>();
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public int TicketTypeId { get; set; }
        public DateTime ExpiryDateTime { get; set; }

        public TicketType TicketType { get; set; }
        public ICollection<TicketToTags> TicketToTags { get; set; }
    }
}
