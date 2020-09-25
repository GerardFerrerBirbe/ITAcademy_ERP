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
    public class PeopleController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;
        private readonly AddressesController _addressesController;

        public PeopleController(
            ITAcademyERPContext context,
            AddressesController addressesController)
        {
            _context = context;
            _addressesController = addressesController;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.Person
                .ToListAsync();
        }

        public async Task<IActionResult> UpdatePerson(int id, PersonDTO personDTO)
        {            
            var person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            person.Email = personDTO.Email;
            person.FirstName = personDTO.FirstName;
            person.LastName = personDTO.LastName;

            _context.Entry(person).State = EntityState.Modified;

            await _addressesController.UpdateAddress(person.PersonalAddressId, personDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
