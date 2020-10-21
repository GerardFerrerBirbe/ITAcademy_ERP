using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Address : IEntity<Guid>
    {
        public Address()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
