using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class OrderLine : IEntity<Guid>
    {
        public OrderLine()
        {            
        }
        
        public Guid Id { get; set; }
        public Guid OrderHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public double Quantity { get; set; }

        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Product Product { get; set; }
    }
}
