using ITAcademyERP.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class Employee : IEntity<Guid>
    {
        public Employee()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required, ForeignKey("PersonId")]
        public string PersonId { get; set; }
        [StringLength(30)]
        public string Position { get; set; }
        public double Salary { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
