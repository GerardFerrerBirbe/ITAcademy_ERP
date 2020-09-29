using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize (Roles = "Admin")]
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

                var roleUsers = new List<UserDTO>();

                foreach (var user in userManager.Users)
                {
                    var roleUser = new UserDTO
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };                  

                    roleUsers.Add(roleUser);                                       
                }

                var roleDTO = new RoleDTO
                {
                    RoleId = id,
                    RoleName = identityRole.Name,
                    RoleUsers = roleUsers
                };

                return roleDTO;
            }
            else
            {
                identityRole = await roleManager
                    .FindByIdAsync(id);

                var roleDTO = new RoleDTO
                {
                    RoleId = id,
                    RoleName = identityRole.Name
                };

                return roleDTO;
            }
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, IdentityRole identityRole)
        {
            if (id != identityRole.Id)
            {
                return BadRequest();
            }

            var role = await roleManager.FindByIdAsync(id);

            role.Name = identityRole.Name;
            //role.NormalizedName = identityRole.Name.Normalize();

            await roleManager.UpdateAsync(role);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = role.Name
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return CreatedAtAction("GetRole", new { id = identityRole.Id }, role);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return NoContent();
        }

        [Route("Users")]
        [HttpGet]
        //public async Task<ActionResult<List<RoleDTO>>> GetUsersInRole (string roleId)
        //{
        //    var role = await roleManager.FindByIdAsync(roleId);

        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new List<RoleDTO>();

        //    foreach (var user in userManager.Users)
        //    {
        //        var userRole = new RoleDTO
        //        {
        //            UserId = user.Id,
        //            UserName = user.UserName
        //        };

        //        if (await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRole.IsSelected = true;
        //        }
        //        else
        //        {
        //            userRole.IsSelected = false;
        //        }

        //        model.Add(userRole);
        //    }

        //    return model;
        //}

        //[Route("Users")]
        //[HttpPost]
        //public async Task<IActionResult> EditUsersInRole(List<RoleDTO> model, string roleId)
        //{
        //    var role = await roleManager.FindByIdAsync(roleId);

        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    for (int i = 0; i < model.Count; i++)
        //    {
        //        var user = await userManager.FindByIdAsync(model[i].UserId);

        //        IdentityResult result = null;

        //        if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
        //        {
        //            result = await userManager.AddToRoleAsync(user, role.Name);
        //        }
        //        else if(!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            result = await userManager.RemoveFromRoleAsync(user, role.Name);
        //        }
        //        else
        //        {
        //            continue;
        //        }

        //        if (result.Succeeded)
        //        {
        //            if (i < (model.Count - 1))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                return RedirectToAction("EditRole", new { Id = roleId });
        //            }
        //        }
        //    }

        //    return RedirectToAction("EditRole", new { Id = roleId });
        //}

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
    }
}
