using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Address
    {
        public Address()
        {
            OrderHeader = new HashSet<OrderHeader>();
            Person = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string AddressName { get; set; }

        public virtual ICollection<OrderHeader> OrderHeader { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}
