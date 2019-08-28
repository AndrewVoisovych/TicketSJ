using System;
using System.Collections.Generic;

namespace WindowsService.Entity
{
    public partial class TicketTags
    {
        public TicketTags()
        {
            TicketToTags = new HashSet<TicketToTags>();
        }

        public int TagId { get; set; }
        public string TagTitle { get; set; }

        public ICollection<TicketToTags> TicketToTags { get; set; }
    }
}
