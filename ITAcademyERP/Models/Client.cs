﻿using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Client
    {
        public Client()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
