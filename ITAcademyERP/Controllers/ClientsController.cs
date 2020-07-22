using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public ClientsController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClient()
        {
            return await _context.Client
                .Include(c => c.Person)
                .Select(c => ClientToDTO(c))
                .ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _context.Client
                .Include(c => c.Person)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return ClientToDTO(client);
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, ClientDTO clientDTO)
        {
            if (id != clientDTO.Id)
            {
                return BadRequest();
            }

            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(client.PersonId);

            if (person == null)
            {
                return NotFound();
            }
            
            person.FirstName = clientDTO.FirstName;
            person.LastName = clientDTO.LastName;

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(ClientDTO clientDTO)
        {
            var person = new Person
            {
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName
            };

            _context.Person.Add(person);

            await _context.SaveChangesAsync();

            var client = new Client
            {
                PersonId = clientDTO.PersonId
            };

            _context.Client.Add(client);            

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, ClientToDTO(client));
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }

        private static ClientDTO ClientToDTO(Client client) =>
            new ClientDTO
            {
                Id = client.Id,
                PersonId = client.PersonId,
                FirstName = client.Person.FirstName,
                LastName = client.Person.LastName
            };
    }
}
