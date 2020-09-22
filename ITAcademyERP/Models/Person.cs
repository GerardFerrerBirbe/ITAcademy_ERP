using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Person
    {
        public Person()
        {
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PersonalAddressId { get; set; }

        public virtual Address PersonalAddress { get; set; }
        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
