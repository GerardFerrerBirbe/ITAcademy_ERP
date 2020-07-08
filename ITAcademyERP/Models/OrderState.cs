using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class OrderState
    {
        public OrderState()
        {
            OrderHeader = new HashSet<OrderHeader>();

        }
        public int Id { get; set; }
        public string State { get; set; }

        public virtual ICollection<OrderHeader> OrderHeader { get; set; }
    }
}
