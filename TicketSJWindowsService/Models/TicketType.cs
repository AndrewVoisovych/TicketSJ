﻿using System;
using System.Collections.Generic;

namespace TicketSJWindowsService.Models
{
    public partial class TicketType
    {
        public TicketType()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public ICollection<Ticket> Ticket { get; set; }
    }
}
