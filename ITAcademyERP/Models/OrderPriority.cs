using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class OrderPriority
    {
        public OrderPriority()
        {
            OrderHeaders = new HashSet<OrderHeader>();

        }
        public int Id { get; set; }
        public string Priority { get; set; }

        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
