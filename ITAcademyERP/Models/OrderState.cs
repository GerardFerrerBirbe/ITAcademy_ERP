using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class OrderState : IEntity
    {
        public OrderState()
        {
            OrderHeaders = new HashSet<OrderHeader>();

        }
        public int Id { get; set; }
        public string State { get; set; }

        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
