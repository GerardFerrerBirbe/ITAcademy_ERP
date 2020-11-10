using ITAcademyERP.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
        
        [ForeignKey("PersonId")]
        public string PersonId { get; set; }

        [StringLength(30, ErrorMessage = "La posició ha de tenir menys de 30 caràcters")]
        public string Position { get; set; }
        
        public double Salary { get; set; }

        
        public virtual Person Person { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
