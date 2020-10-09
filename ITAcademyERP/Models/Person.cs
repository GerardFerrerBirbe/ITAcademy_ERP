using ITAcademyERP.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Person: IdentityUser, IEntity
    {
        public Person()
        {
            Addresses = new HashSet<Address>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
