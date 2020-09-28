using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Person
    {
        public Person()
        {
            Addresses = new HashSet<Address>();

        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
