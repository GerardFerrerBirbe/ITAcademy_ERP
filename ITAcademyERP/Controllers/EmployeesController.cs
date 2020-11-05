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
using ITAcademyERP.Data.DTOs;

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
        private readonly AddressesController _addressesController;
        private readonly UserManager<Person> _userManager;

        public EmployeesController(
            EmployeesRepository repository,
            PeopleController peopleController,
            PeopleRepository peopleRepository,
            ClientsRepository clientsRepository,
            AddressesController addressesController,
            UserManager<Person> userManager) : base(repository)
        {
            _repository = repository;
            _peopleRepository = peopleRepository;
            _peopleController = peopleController;
            _clientsRepository = clientsRepository;
            _addressesController = addressesController;
            _userManager = userManager;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var employees = await _repository.GetAll();

            return employees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            var employee = await _repository.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Employee employeeUpdate)
        {
            var personId = (await _peopleRepository.GetPerson(employeeUpdate.Person.Id)).Id;
            
            var person = new Person
            {
                Id = personId,
                Email = employeeUpdate.Person.Email,
                FirstName = employeeUpdate.Person.FirstName,
                LastName = employeeUpdate.Person.LastName,
                Addresses = employeeUpdate.Person.Addresses
            };

            var updatePerson = await _peopleController.UpdatePerson(person);

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
                var employee = await _repository.Get(employeeUpdate.Id);

                employee.Position = employeeUpdate.Position;
                employee.Salary = employeeUpdate.Salary;

                return await _repository.Update(employee);
            }            
        }

        // POST: api/Employees
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostEmployee(Employee newEmployee)
        {
            var person = await _peopleRepository.GetPersonByEmail(newEmployee.Person.Email);

            if (person == null)
            {
                var newPerson = new Person
                {
                    Email = newEmployee.Person.Email,
                    FirstName = newEmployee.Person.FirstName,
                    LastName = newEmployee.Person.LastName,
                    Addresses = newEmployee.Person.Addresses
                };

                await _peopleController.AddPerson(newPerson);

                var getPerson = await _peopleRepository.GetPersonByEmail(newPerson.Email);
                getPerson.UserName = newPerson.Email;

                await _userManager.UpdateAsync(getPerson);
            }
            else
            {
                person.FirstName = newEmployee.Person.FirstName;
                person.LastName = newEmployee.Person.LastName;

                await _peopleRepository.Update(person);
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
        public async Task<ActionResult<Employee>> DeleteEmployee(Guid id)
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
