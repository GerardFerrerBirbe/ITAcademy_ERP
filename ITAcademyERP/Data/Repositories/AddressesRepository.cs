using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class AddressesRepository : GenericRepository<Guid, Address, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public AddressesRepository(
            ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Address> GetAddress(Guid id)
        {
            return await _context.Addresses
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public IActionResult DeleteList([FromBody] List<string> ids)
        {
            try
            {
                List<Address> addresses = ids.Select(id => new Address() { Id = Guid.Parse(id) }).ToList();
                _context.RemoveRange(addresses);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
