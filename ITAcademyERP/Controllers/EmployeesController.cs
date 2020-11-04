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
        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            var employees = await _repository.GetAll();

            return employees.Select(e => EmployeeToDTO(e));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(Guid id)
        {
            var employee = await _repository.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            return EmployeeToDTO(employee);
        }

        // PUT: api/Employees/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(EmployeeDTO employeeDTO)
        {
            var personId = (await _peopleRepository.GetPerson(employeeDTO.PersonId)).Id;
            
            var personDTO = new PersonDTO
            {
                Id = personId,
                Email = employeeDTO.Email,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Addresses = employeeDTO.Addresses
            };

            var updatePerson = await _peopleController.UpdatePerson(personDTO);

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
                var employee = await _repository.Get(employeeDTO.Id);

                employee.Position = employeeDTO.Position;
                employee.Salary = employeeDTO.Salary;

                return await _repository.Update(employee);
            }            
        }

        // POST: api/Employees
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostEmployee(EmployeeDTO employeeDTO)
        {
            var person = await _peopleRepository.GetPersonByEmail(employeeDTO.Email);

            if (person == null)
            {
                var personDTO = new PersonDTO
                {
                    Email = employeeDTO.Email,
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    Addresses = employeeDTO.Addresses
                };

                await _peopleController.AddPerson(personDTO);

                var newPerson = await _peopleRepository.GetPersonByEmail(personDTO.Email);

                newPerson.UserName = personDTO.Email;

                await _userManager.UpdateAsync(newPerson);
            }
            else
            {
                person.FirstName = employeeDTO.FirstName;
                person.LastName = employeeDTO.LastName;

                await _peopleRepository.Update(person);
            }

            var employee = new Employee
            {
                PersonId = _peopleRepository.GetPersonId(employeeDTO.Email),
                Position = employeeDTO.Position,
                Salary = employeeDTO.Salary
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

        private EmployeeDTO EmployeeToDTO(Employee employee) =>
            new EmployeeDTO
            {
                Id = employee.Id,
                PersonId = employee.PersonId,
                Email = employee.Person.Email,
                FirstName = employee.Person.FirstName,
                LastName = employee.Person.LastName,
                Addresses = employee.Person.Addresses,
                Position = employee.Position,
                Salary = employee.Salary
            };        
    }
}
