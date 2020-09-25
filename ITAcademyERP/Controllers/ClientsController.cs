using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Controllers;
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
            return await _context.Client
                .Include(c => c.Person)
                .ThenInclude(p => p.PersonalAddress)
                .Select(c => ClientToDTO(c))
                .ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _context.Client
                .Include(c => c.Person)
                .ThenInclude(p => p.PersonalAddress)
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

            var personDTO = new PersonDTO
            {
                Email = clientDTO.Email,
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName,
                Address = clientDTO.Address
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
            var addressExists = _context.Address.FirstOrDefault(a => a.AddressName == clientDTO.Address);

            if (addressExists == default)
            {
                var address = new Address
                {
                    AddressName = clientDTO.Address
                };

                _context.Address.Add(address);

                _context.SaveChanges();
            }

            var addressId = _context.Address.FirstOrDefault(a => a.AddressName == clientDTO.Address).Id;

            var personExists = _context.Person.FirstOrDefault(p => p.FirstName + ' ' + p.LastName == clientDTO.FirstName + ' ' + clientDTO.LastName);

            if (personExists == default)
            {
                var person = new Person
                {
                    Email = clientDTO.Email,
                    FirstName = clientDTO.FirstName,
                    LastName = clientDTO.LastName,
                    PersonalAddressId = addressId
                };

                _context.Person.Add(person);

                _context.SaveChanges();
            }

            var personId = _context.Person.FirstOrDefault(p => p.Email == clientDTO.Email).Id;

            var client = new Client
            {
                PersonId = personId
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
                Email = client.Person.Email,
                FirstName = client.Person.FirstName,
                LastName = client.Person.LastName,
                Address = client.Person.PersonalAddress.AddressName
            };

        public int GetClientId (string clientName)
        {
            var personClientId = _context.Person
                            .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == clientName)
                            .Id;

            var clientId = _context.Client
                            .FirstOrDefault(x => x.PersonId == personClientId)
                            .Id;

            return clientId;
        }
    }
}
