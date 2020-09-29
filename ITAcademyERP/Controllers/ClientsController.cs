using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Controllers;
using ITAcademyERP.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Models
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Employee")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;
        private readonly PeopleController _peopleController;

        public ClientsController(
            ITAcademyERPContext context,
            PeopleController peopleController)
        {
            _context = context;
            _peopleController = peopleController;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            return await _context.Clients
                .Include(c => c.Person)
                .ThenInclude(p => p.Addresses)
                .Select(c => ClientToDTO(c))
                .ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Person)
                .ThenInclude(p => p.Addresses)
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

            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            var personDTO = new PersonDTO
            {
                Email = clientDTO.Email,
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName,
                Addresses = clientDTO.Addresses
            };

            await _peopleController.UpdatePerson(client.PersonId, personDTO);

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
            var personExists = _context.People.FirstOrDefault(p => p.FirstName + ' ' + p.LastName == clientDTO.FirstName + ' ' + clientDTO.LastName);

            if (personExists == default)
            {
                var person = new Person
                {
                    Email = clientDTO.Email,
                    FirstName = clientDTO.FirstName,
                    LastName = clientDTO.LastName
                };

                _context.People.Add(person);

                _context.SaveChanges();
            }

            var personId = _context.People.FirstOrDefault(p => p.Email == clientDTO.Email).Id;

            var client = new Client
            {
                PersonId = personId
            };

            _context.Clients.Add(client);            

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, ClientToDTO(client));
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        private static ClientDTO ClientToDTO(Client client) =>
            new ClientDTO
            {
                Id = client.Id,
                PersonId = client.PersonId,
                Email = client.Person.Email,
                FirstName = client.Person.FirstName,
                LastName = client.Person.LastName,
                Addresses = client.Person.Addresses.Select(a => AddressesController.AddressToDTO(a)).ToList()
            };       
        
        public int GetClientId (string clientName)
        {
            var personClientId = _context.People
                            .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == clientName)
                            .Id;

            var clientId = _context.Clients
                            .FirstOrDefault(x => x.PersonId == personClientId)
                            .Id;

            return clientId;
        }
    }
}
