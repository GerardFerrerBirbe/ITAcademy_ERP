using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
using ITAcademyERP.Data.Resources;
using ITAcademyERP.Data.Repositories;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class AddressesController : GenericController<Guid, Address, AddressesRepository>
    {
        private readonly AddressesRepository _repository;

        public AddressesController(
            AddressesRepository repository) : base(repository)
        {
            _repository = repository;
        }        

        public async Task CreateOrEditAddresses(ICollection<Address> addresses)
        {
            ICollection<Address> addressesToCreate = addresses.Where(x => x.Id == Guid.Empty).ToList();
            ICollection<Address> addressesToEdit = addresses.Where(x => x.Id != Guid.Empty).ToList();

            if (addressesToCreate.Any())
            {
                foreach (var addressToCreate in addressesToCreate)
                {
                    if (addressToCreate.Name == "")
                        return;

                    await _repository.Add(addressToCreate);
                }
            }

            if (addressesToEdit.Any())
            {
                foreach (var addressToEdit in addressesToEdit)
                {
                    await _repository.Update(addressToEdit);                   
                }
            }
        }

        // POST: api/Addresses
        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteList([FromBody] List<string> addresses)
        {
            return _repository.DeleteList(addresses);
        }
    }
}
