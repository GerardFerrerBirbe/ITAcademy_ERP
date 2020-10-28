using System;
using System.Collections.Generic;

namespace ITAcademyERP.Data.DTOs
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserDTO> RoleUsers { get; set; }
    }
}
