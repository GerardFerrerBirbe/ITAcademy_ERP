using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class OrderLine : IEntity<Guid>
    {
        public OrderLine()
        {            
        }
        
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("OrderHeaderId")]
        public Guid OrderHeaderId { get; set; }
        
        [Required]
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }
        
        [Required]
        public double UnitPrice { get; set; }
        
        [Required]
        public double Vat { get; set; }
        
        [Required]
        public double Quantity { get; set; }


        public virtual OrderHeader OrderHeader { get; set; }
        
        public virtual Product Product { get; set; }
    }
}
