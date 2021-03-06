﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data.Resources;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITAcademyERP.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ApiController]    
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Person> _userManager;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<Person> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //GET: api/Roles
        [HttpGet]
        public IQueryable<IdentityRole> GetRoles()
        {
            var roles = _roleManager.Roles;           
            
            return roles;
        }

        //GET: api/Roles/RoleUsers
        [Route("RoleUsers")]
        [HttpGet]
        public List<UserDTO> GetRoleUsers()
        {
            var users = _userManager.Users;

            var roleUsers = new List<UserDTO>();
            
            foreach (var user in users)
            {
                var roleUserDTO = new UserDTO
                {
                    Name = user.FirstName + " " + user.LastName,
                    Email = user.Email
                };

                roleUsers.Add(roleUserDTO);
            }

            return roleUsers;
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
                identityRole = await _roleManager
                    .FindByIdAsync(id);

                if (identityRole == null)
                {
                    return NotFound();
                }

                var usersInRoleDTO = new List<UserDTO>();

                var usersInRole = await _userManager.GetUsersInRoleAsync(identityRole.Name);

                foreach (var user in usersInRole)
                {
                    var roleUserDTO = new UserDTO
                    {
                        Id = user.Id,
                        RoleId = identityRole.Id,
                        Name = user.FirstName + " " + user.LastName,
                        Email = user.Email
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
                identityRole = await _roleManager
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
       [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, RoleDTO roleDTO)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (roleDTO.Name != role.Name && await RoleExists(roleDTO.Name))
            {
                ModelState.AddModelError(string.Empty, "Role ja existent");
                return BadRequest(ModelState);
            }

            role.Name = roleDTO.Name;

            await _roleManager.UpdateAsync(role);           

            return NoContent();
        }        

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDTO roleDTO)
        {
            if (!await RoleExists(roleDTO.Name))
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = roleDTO.Name
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return CreatedAtAction("GetRole", new { id = identityRole.Id }, roleDTO);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ModelState.AddModelError(string.Empty, "Role ja existent");
            return BadRequest(ModelState);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IdentityRole>> DeleteRole(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await _roleManager.DeleteAsync(role);

            return role;
        }

        // PUT: api/Roles
        [HttpPut]
        public async Task<IActionResult> UpdateRoleUser(UserDTO userDTO, string addOrRemove)
        {
            if (addOrRemove == "add")
            {
                var role = await _roleManager.FindByIdAsync(userDTO.RoleId);

                var splitAction = userDTO.Name.Split("- ");

                var email = splitAction[1];

                var user = await _userManager.FindByEmailAsync(email);

                await _userManager.AddToRoleAsync(user, role.Name);
            }

            if (addOrRemove == "remove")
            {
                var role = await _roleManager.FindByIdAsync(userDTO.RoleId);

                var user = await _userManager.FindByEmailAsync(userDTO.Email);

                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            return NoContent();            
        }

        public async Task<bool> RoleExists(string name)
        {
            var roleExists = await _roleManager.RoleExistsAsync(name);
            return roleExists;
        }
    }
}
