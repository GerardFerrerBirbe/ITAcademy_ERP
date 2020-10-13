using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ITAcademyERP.Controllers;
using ITAcademyERP.Data;
using ITAcademyERP.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Models
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Employee")]
    [ApiController]
    public class ClientsController : GenericController<Client, ClientsRepository>
    {
        private readonly ClientsRepository _repository;
        private readonly PeopleController _peopleController;
        private readonly PeopleRepository _peopleRepository;

        public ClientsController(
            ClientsRepository repository,
            PeopleController peopleController,
            PeopleRepository peopleRepository) : base(repository)
        {
            _repository = repository;
            _peopleController = peopleController;
            _peopleRepository = peopleRepository;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<IEnumerable<ClientDTO>> GetClients()
        {
            var client = await _repository.GetClients();

            return client.Select(c => ClientToDTO(c));
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(int id)
        {
            var client = await _repository.GetClient(id);

            if (client == null)
            {
                return NotFound();
            }

            return ClientToDTO(client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(ClientDTO clientDTO)
        {
            var personId = (await _peopleRepository.GetPerson(clientDTO.PersonId)).Id;

            var personDTO = new PersonDTO
            {
                Id = personId,
                Email = clientDTO.Email,
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName,
                Addresses = clientDTO.Addresses
            };

            return await _peopleController.UpdatePerson(personDTO);
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(ClientDTO clientDTO)
        {               
            var person = new Person
            {
                Email = clientDTO.Email,
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName
            };            
            
            var addPerson = await _peopleRepository.Add(person);

            var statusCode = GetHttpStatusCode(addPerson).ToString();

            if (statusCode != "OK")
            {
                return addPerson;
            }
            else
            {
                var client = new Client
                {
                    PersonId = _peopleRepository.GetPersonId(clientDTO.Email)
                };

                return await _repository.Add(client);
            }           
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
    }
}
