using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using ITAcademyERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using ITAcademyERP.Data.Repositories;
using System.Net;
using ITAcademyERP.Data.Resources;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class EmployeesController : GenericController<Guid, Employee, EmployeesRepository>
    {
        private readonly EmployeesRepository _repository;
        private readonly PeopleController _peopleController;
        private readonly PeopleRepository _peopleRepository;
        private readonly ClientsRepository _clientsRepository;
        private readonly UserManager<Person> _userManager;

        public EmployeesController(
            EmployeesRepository repository,
            PeopleController peopleController,
            PeopleRepository peopleRepository,
            ClientsRepository clientsRepository,
            UserManager<Person> userManager) : base(repository)
        {
            _repository = repository;
            _peopleRepository = peopleRepository;
            _peopleController = peopleController;
            _clientsRepository = clientsRepository;
            _userManager = userManager;
        }

        // PUT: api/Employees/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public override async Task<IActionResult> Put(Employee employee)
        {
            var updatePerson = await _peopleController.UpdatePerson(employee.Person);

            var statusCode = updatePerson
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(updatePerson, null).ToString();

            if (statusCode != "200")
            {
                return updatePerson;
            }
            else
            {
                return await _repository.Update(employee);
            }            
        }

        // POST: api/Employees
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public override async Task<ActionResult> Post(Employee newEmployee)
        {
            var person = await _peopleRepository.GetPersonByEmail(newEmployee.Person.Email);

            if (person == null)
            {
                await _peopleController.AddPerson(newEmployee.Person);

                var getPerson = await _peopleRepository.GetPersonByEmail(newEmployee.Person.Email);
                getPerson.UserName = newEmployee.Person.Email;

                await _userManager.UpdateAsync(getPerson);
            }
            else
            {
                newEmployee.Person.Id = person.Id;
                foreach (var address in newEmployee.Person.Addresses)
                {
                    address.PersonId = person.Id;
                }

                await _peopleController.UpdatePerson(newEmployee.Person);
            }

            var employee = new Employee
            {
                PersonId = _peopleRepository.GetPersonId(newEmployee.Person.Email),
                Position = newEmployee.Position,
                Salary = newEmployee.Salary
            };

            return await _repository.Add(employee);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public override async Task<ActionResult<Employee>> Delete(Guid id)
        {
            var employee = await _repository.Delete(id);
            
            if (employee == null)
            {
                return NotFound();
            }

            var client = await _clientsRepository.GetClient(employee.PersonId);
            
            if (client == null)
            {
                await _peopleRepository.Delete(employee.PersonId);
            }

            return employee;
        }    
    }
}
