using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string PersonId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<AddressDTO> Addresses { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }

    }
}
