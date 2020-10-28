using System;
using System.ComponentModel.DataAnnotations;

namespace ITAcademyERP.Models
{
    public class OrderLineDTO
    {
        public Guid Id { get; set; }

        public Guid OrderHeaderId { get; set; }

        public string ProductName { get; set; }

        public double UnitPrice { get; set; }

        public double Vat { get; set; }

        public double Quantity { get; set; }
    }
}
