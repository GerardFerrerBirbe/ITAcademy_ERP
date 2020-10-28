using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITAcademyERP.Data.DTOs
{
    public class OrderHeaderDTO
    {        
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Camp requerit")]
        [MaxLength(20, ErrorMessage = "Ha de tenir com a màxim 20 caràcters")]
        public string OrderNumber { get; set; }
        
        public string Address { get; set; }

        public string Client { get; set; }
        
        public string Employee { get; set; }
        
        public string OrderState { get; set; }
        
        public string OrderPriority { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime AssignToEmployeeDate { get; set; }
        
        public DateTime? FinalisationDate { get; set; }

        public ICollection<OrderLineDTO> OrderLines { get; set; }

        public string YearMonth { get; set; }

        public double TotalSales { get; set; }
    }
}
