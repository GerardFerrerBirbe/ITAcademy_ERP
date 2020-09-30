using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserDTO> RoleUsers { get; set; }
    }
}
