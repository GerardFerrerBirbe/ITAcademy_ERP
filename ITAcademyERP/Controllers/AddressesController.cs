using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> UpdateAddress(int? id, PersonDTO personDTO)
        {
            var address = await _context.Address.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            address.AddressName = personDTO.Address;

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AddressExists(int? id)
        {
            return _context.Address.Any(e => e.Id == id);
        }

        public int GetAddressId (string addressName)
        {
            var addressId = _context.Address
                            .FirstOrDefault(x => x.AddressName == addressName)
                            .Id;

            return addressId;
        }
    }
}
