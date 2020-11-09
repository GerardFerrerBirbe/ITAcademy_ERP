using ITAcademyERP.Data;
using ITAcademyERP.Data.Resources;
using Newtonsoft.Json;
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

        //[Required(ErrorMessage = "Camp requerit")]
        [MaxLength(20, ErrorMessage = "Ha de tenir com a màxim 20 caràcters")]
        public string OrderNumber { get; set; }
        
        [JsonIgnore]
        //[Required]
        [ForeignKey("ClientId")]
        public Guid ClientId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("EmployeeId")]
        public Guid EmployeeId { get; set; }
        
        public EOrderState OrderState { get; set; }
        
        public EOrderPriority OrderPriority { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime AssignToEmployeeDate { get; set; }
        
        public DateTime? FinalisationDate { get; set; }

        
        
        public virtual Client Client { get; set; }
        
        public virtual Employee Employee { get; set; }
        
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
