using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
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
    public class AddressesController : GenericController<Address, AddressesRepository>
    {
        private readonly ITAcademyERPContext _context;
        private readonly AddressesRepository _repository;

        public AddressesController(
            ITAcademyERPContext context,
            AddressesRepository repository) : base(repository)
        {
            _context = context;
            _repository = repository;
        }        

        public async Task CreateOrEditAddresses(ICollection<AddressDTO> addressesDTO)
        {
            ICollection<AddressDTO> addressesToCreate = addressesDTO.Where(x => x.Id == 0).ToList();
            ICollection<AddressDTO> addressesToEdit = addressesDTO.Where(x => x.Id != 0).ToList();

            if (addressesToCreate.Any())
            {
                foreach (var addressToCreate in addressesToCreate)
                {
                    if (addressToCreate.Name == "")
                        return;

                    var address = new Address
                    {
                        Id = addressToCreate.Id,
                        PersonId = addressToCreate.PersonId,
                        Name = addressToCreate.Name,
                        Type = addressToCreate.Type
                    };

                    await _repository.Add(address);
                }
            }

            if (addressesToEdit.Any())
            {
                foreach (var addressToEdit in addressesToEdit)
                {
                    var address = await _repository.GetAddress(addressToEdit.Id);

                    address.Id = addressToEdit.Id;
                    address.PersonId = addressToEdit.PersonId;
                    address.Name = addressToEdit.Name;
                    address.Type = addressToEdit.Type;

                    await _repository.Update(address);                   
                }
            }
        }

        // POST: api/Addresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public IActionResult DeleteList([FromBody] List<int> ids)
        {
            return _repository.DeleteList(ids);
        }

        public static AddressDTO AddressToDTO(Address address)
        {
            var addressDTO = new AddressDTO
            {
                Id = address.Id,
                PersonId = address.PersonId,
                Name = address.Name,
                Type = address.Type
            };

            return addressDTO;
        }
    }
}
