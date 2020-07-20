using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class OrderLineDTO
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public double Quantity { get; set; }
    }
}
