using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public AddressesController(ITAcademyERPContext context)
        {
            _context = context;
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

                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();

                    CreatedAtAction("GetAddress", new { id = address.Id }, AddressToDTO(address));
                }
            }

            if (addressesToEdit.Any())
            {
                foreach (var addressToEdit in addressesToEdit)
                {
                    var address = _context.Addresses.FirstOrDefault(x => x.Id == addressToEdit.Id);

                    address.Id = addressToEdit.Id;
                    address.PersonId = addressToEdit.PersonId;
                    address.Name = addressToEdit.Name;
                    address.Type = addressToEdit.Type;

                    _context.Entry(address).State = EntityState.Modified;
                }
            }
        }

        // POST: api/Addresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public IActionResult DeleteList([FromBody] List<int> ids)
        {
            try
            {
                List<Address> addresses = ids.Select(id => new Address() { Id = id }).ToList();
                _context.RemoveRange(addresses);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
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

        public int GetAddressId (string addressName)
        {
            var addressId = _context.Addresses
                            .FirstOrDefault(x => x.Name == addressName)
                            .Id;

            return addressId;
        }
    }
}
