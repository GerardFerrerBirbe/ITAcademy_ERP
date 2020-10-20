﻿using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class OrderHeader : IEntity
    {
        public OrderHeader()
        {
            OrderLines = new HashSet<OrderLine>();
        }
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public OrderState OrderState { get; set; }
        public OrderPriority OrderPriority { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime AssignToEmployeeDate { get; set; }
        public DateTime? FinalisationDate { get; set; }


        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
