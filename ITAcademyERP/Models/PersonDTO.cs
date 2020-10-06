using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public class PersonDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<AddressDTO> Addresses { get; set; }
    }
}
