using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Client : IEntity<Guid>
    {
        public Client()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        public Guid Id { get; set; }
        public string PersonId { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
