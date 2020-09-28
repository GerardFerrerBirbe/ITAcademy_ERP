﻿using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Address
    {
        public Address()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
