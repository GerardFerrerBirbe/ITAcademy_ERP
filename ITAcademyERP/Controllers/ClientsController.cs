using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ITAcademyERP.Controllers;
using ITAcademyERP.Data;
using ITAcademyERP.Data.Resources;
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
        private readonly UserManager<Person> _userManager;

        public ClientsController(
            ClientsRepository repository,
            PeopleController peopleController,
            PeopleRepository peopleRepository,
            EmployeesRepository employeesRepository,
            UserManager<Person> userManager) : base(repository)
        {
            _repository = repository;
            _peopleController = peopleController;
            _peopleRepository = peopleRepository;
            _employeesRepository = employeesRepository;
            _userManager = userManager;
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public override async Task<IActionResult> Put(Client client)
        {
            return await _peopleController.UpdatePerson(client.Person);
        }

        // POST: api/Clients
        [HttpPost]
        public override async Task<ActionResult> Post(Client newClient)
        {
            var person = await _peopleRepository.GetPersonByEmail(newClient.Person.Email);

            if (person == null)
            {
                await _peopleController.AddPerson(newClient.Person);

                var getPerson = await _peopleRepository.GetPersonByEmail(newClient.Person.Email);
                getPerson.UserName = newClient.Person.Email;

                await _userManager.UpdateAsync(getPerson);
            }
            else
            {
                newClient.Person.Id = person.Id;
                foreach (var address in newClient.Person.Addresses)
                {
                    address.PersonId = person.Id;
                }

                await _peopleController.UpdatePerson(newClient.Person);
            }      
                       
            var client = new Client
            {
                PersonId = _peopleRepository.GetPersonId(newClient.Person.Email)
            };

            return await _repository.Add(client);                    
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public override async Task<ActionResult<Client>> Delete(Guid id)
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
