using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class OrderHeader : IEntity<Guid>
    {
        public OrderHeader()
        {
            OrderLines = new HashSet<OrderLine>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        [Required, ForeignKey("ClientId")]
        public Guid ClientId { get; set; }
        [ForeignKey("EmployeeId")]
        public Guid EmployeeId { get; set; }
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
