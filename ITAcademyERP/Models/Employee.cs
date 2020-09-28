using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Employee
    {
        public Employee()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
