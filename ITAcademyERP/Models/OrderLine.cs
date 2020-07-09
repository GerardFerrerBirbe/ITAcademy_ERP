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
        public int? OrderHeaderId { get; set; }
        public string ProductId { get; set; }
        public double? UnitNetPrice { get; set; }
        public double? UnitVatPrice { get; set; }
        public double? Quantity { get; set; }
        public double? TotalNetPrice { get; set; }
        public double? TotalVatPrice { get; set; }

        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Product Product { get; set; }
    }
}
