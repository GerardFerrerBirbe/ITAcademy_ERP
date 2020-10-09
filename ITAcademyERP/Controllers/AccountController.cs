using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ITAcademyERP.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ITAcademyERP.Data;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Data.Repositories;

namespace ITAcademyERP.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly PeopleRepository _peopleRepository;

        public AccountController(
            UserManager<Person> userManager,
            SignInManager<Person> signInManager,
            IConfiguration configuration,
            PeopleRepository peopleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _peopleRepository = peopleRepository;
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreatePassword([FromBody] UserInfo model)
        {            
            var user = await _peopleRepository.GetPersonByEmail(model.Email);

            if (user == default)
            {
                ModelState.AddModelError(string.Empty, "Email no registrat. Contacta amb l'administrador");
                return BadRequest(ModelState);
            }
                
            if (user.PasswordHash != null)
            {
                ModelState.AddModelError(string.Empty, "Usuari ja registrat. Clica al botó de 'Login'");
                return BadRequest(ModelState);
            }

            var hasher = new PasswordHasher<IdentityUser>();

            user.PasswordHash = hasher.HashPassword(user, model.Password);

            await _peopleRepository.Update(user);
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                return await BuildToken(model, roles);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }                

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        {            
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(userInfo.Email);

                var roles = await _userManager.GetRolesAsync(user);
                    
                return await BuildToken(userInfo, roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Contrasenya incorrecte");
                return BadRequest(ModelState);
            }            
        }

        private async Task<IActionResult> BuildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                        
            foreach (var role in roles)
            {                
                claims.Add(new Claim(ClaimTypes.Role, role));                
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Llave_super_secreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(7);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "yourdomain.com",
               audience: "yourdomain.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            var user = await _peopleRepository.GetPersonByEmail(userInfo.Email);
            var userName = user.FirstName + ' ' + user.LastName;
            var isAdminUser = roles.Contains("Admin");
            
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration,
                userName = userName,
                isAdminUser = isAdminUser
            });
        }        
    }
}
