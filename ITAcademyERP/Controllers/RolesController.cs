using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ApiController]    
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IQueryable<IdentityRole> GetRoles()
        {
            var roles = roleManager.Roles;           
            
            return roles;
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(string id, bool includeUsers = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityRole identityRole;
            
            if (includeUsers)
            {
                identityRole = await roleManager
                    .FindByIdAsync(id);

                if (identityRole == null)
                {
                    return NotFound();
                }

                var usersInRoleDTO = new List<UserDTO>();

                var usersInRole = await userManager.GetUsersInRoleAsync(identityRole.Name);

                foreach (var user in usersInRole)
                {
                    var roleUserDTO = new UserDTO
                    {
                        Id = user.Id,
                        RoleId = identityRole.Id,
                        Name = user.UserName
                    };                  

                    usersInRoleDTO.Add(roleUserDTO);                                       
                }

                var roleDTO = new RoleDTO
                {
                    Id = id,
                    Name = identityRole.Name,
                    RoleUsers = usersInRoleDTO
                };

                return roleDTO;
            }
            else
            {
                identityRole = await roleManager
                    .FindByIdAsync(id);

                var roleDTO = new RoleDTO
                {
                    Id = id,
                    Name = identityRole.Name
                };

                return roleDTO;
            }
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, RoleDTO roleDTO)
        {
            if (id != roleDTO.Id)
            {
                return BadRequest();
            }

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            role.Name = roleDTO.Name;

            await roleManager.UpdateAsync(role);

            var usersDTO = roleDTO.RoleUsers;

            foreach (var userDTO in usersDTO)
            {
                var user = await userManager.FindByNameAsync(userDTO.Name);
                await userManager.AddToRoleAsync(user, role.Name);
            }

            return NoContent();
        }        

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = roleDTO.Name
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return CreatedAtAction("GetRole", new { id = identityRole.Id }, roleDTO);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return NoContent();
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IdentityRole>> DeleteRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await roleManager.DeleteAsync(role);

            return role;
        }

        // POST: api/Roles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Route("RoleUsers")]
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteList([FromBody] List<string> ids, string id)
        {
            
            var roleId = id.Substring(9);
            var role = await roleManager.FindByIdAsync(roleId);
            
            try
            {
                foreach (var userId in ids)
                {
                    var roleUser = await userManager.FindByIdAsync(userId);
                    await userManager.RemoveFromRoleAsync(roleUser, role.Name);             
                }                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }        
    }
}
