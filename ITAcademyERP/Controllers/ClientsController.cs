using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ITAcademyERP.Controllers;
using ITAcademyERP.Data;
using ITAcademyERP.Data.DTOs;
using ITAcademyERP.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Models
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class ClientsController : GenericController<Guid, Client, ClientsRepository>
    {
        private readonly ClientsRepository _repository;
        private readonly PeopleController _peopleController;
        private readonly PeopleRepository _peopleRepository;
        private readonly EmployeesRepository _employeesRepository;
        private readonly AddressesController _addressesController;
        private readonly UserManager<Person> _userManager;

        public ClientsController(
            ClientsRepository repository,
            PeopleController peopleController,
            PeopleRepository peopleRepository,
            EmployeesRepository employeesRepository,
            AddressesController addressesController,
            UserManager<Person> userManager) : base(repository)
        {
            _repository = repository;
            _peopleController = peopleController;
            _peopleRepository = peopleRepository;
            _employeesRepository = employeesRepository;
            _userManager = userManager;
            _addressesController = addressesController;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<IEnumerable<ClientDTO>> GetClients()
        {
            var clients = await _repository.GetAll();

            return clients.Select(c => ClientToDTO(c));
        }        

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClient(Guid id)
        {
            var client = await _repository.Get(id);

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
            var person = await _peopleRepository.GetPersonByEmail(clientDTO.Email);

            if (person == null)
            {
                var personDTO = new PersonDTO
                {
                    Email = clientDTO.Email,
                    FirstName = clientDTO.FirstName,
                    LastName = clientDTO.LastName,
                    Addresses = clientDTO.Addresses
                };

                await _peopleController.AddPerson(personDTO);

                var newPerson = await _peopleRepository.GetPersonByEmail(personDTO.Email);

                newPerson.UserName = personDTO.Email;

                await _userManager.UpdateAsync(newPerson);
            }
            else
            {
                person.FirstName = clientDTO.FirstName;
                person.LastName = clientDTO.LastName;

                await _peopleRepository.Update(person);
            }      
                       
            var client = new Client
            {
                PersonId = _peopleRepository.GetPersonId(clientDTO.Email)
            };

            return await _repository.Add(client);                      
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(Guid id)
        {
            var client = await _repository.Delete(id);

            if (client == null)
            {
                return NotFound();
            }

            var employee = await _employeesRepository.GetEmployee(client.PersonId);

            if (employee == null)
            {
                await _peopleRepository.Delete(client.PersonId);
            }

            return client;
        }

        private ClientDTO ClientToDTO(Client client) =>
            new ClientDTO
            {
                Id = client.Id,
                PersonId = client.PersonId,
                Email = client.Person.Email,
                FirstName = client.Person.FirstName,
                LastName = client.Person.LastName,
                Addresses = client.Person.Addresses
            };        
    }
}
