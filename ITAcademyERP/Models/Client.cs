using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class Client : IEntity<Guid>
    {
        public Client()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("PersonId")]
        public string PersonId { get; set; }

        
        public virtual Person Person { get; set; }
        
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
