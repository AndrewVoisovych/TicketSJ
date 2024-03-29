﻿using System;
using System.Collections.Generic;

namespace WindowsService.Entity
{
    public partial class TicketToTags
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int TagId { get; set; }

        public TicketTags Tag { get; set; }
        public Ticket Ticket { get; set; }
    }
}
