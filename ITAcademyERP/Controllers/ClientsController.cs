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
        public async Task<IEnumerable<Client>> GetClients()
        {
            var clients = await _repository.GetAll();

            return clients;
        }        

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(Guid id)
        {
            var client = await _repository.Get(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(Client clientUpdate)
        {
            var personId = (await _peopleRepository.GetPerson(clientUpdate.Person.Id)).Id;

            var person = new Person
            {
                Id = personId,
                Email = clientUpdate.Person.Email,
                FirstName = clientUpdate.Person.FirstName,
                LastName = clientUpdate.Person.LastName,
                Addresses = clientUpdate.Person.Addresses
            };

            return await _peopleController.UpdatePerson(person);
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client newClient)
        {
            var person = await _peopleRepository.GetPersonByEmail(newClient.Person.Email);

            if (person == null)
            {
                var newPerson = new Person
                {
                    Email = newClient.Person.Email,
                    FirstName = newClient.Person.FirstName,
                    LastName = newClient.Person.LastName,
                    Addresses = newClient.Person.Addresses
                };

                await _peopleController.AddPerson(newPerson);

                var getPerson = await _peopleRepository.GetPersonByEmail(newPerson.Email);
                getPerson.UserName = newPerson.Email;

                await _userManager.UpdateAsync(getPerson);
            }
            else
            {
                person.FirstName = newClient.Person.FirstName;
                person.LastName = newClient.Person.LastName;

                await _peopleRepository.Update(person);
            }      
                       
            var client = new Client
            {
                PersonId = _peopleRepository.GetPersonId(newClient.Person.Email)
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
    }
}
