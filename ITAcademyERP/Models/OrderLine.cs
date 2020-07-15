using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class OrderLine
    {
        public OrderLine()
        {            
        }

        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public double Quantity { get; set; }

        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Product Product { get; set; }
    }
}
