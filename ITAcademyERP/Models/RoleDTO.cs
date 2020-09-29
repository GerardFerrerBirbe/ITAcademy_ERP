using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Models
{
    public class RoleDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserDTO> RoleUsers { get; set; }
    }
}
