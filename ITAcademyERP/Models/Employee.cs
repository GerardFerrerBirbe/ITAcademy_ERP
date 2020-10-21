using ITAcademyERP.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Employee : IEntity<Guid>
    {
        public Employee()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        public Guid Id { get; set; }
        public string PersonId { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
