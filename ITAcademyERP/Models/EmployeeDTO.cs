﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<AddressDTO> Addresses { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }

    }
}
