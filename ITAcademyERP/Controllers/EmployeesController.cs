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

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class EmployeesController : GenericController<Employee, EmployeesRepository>
    {
        private readonly EmployeesRepository _repository;
        private readonly PeopleController _peopleController;
        private readonly PeopleRepository _peopleRepository;


        public EmployeesController(
            EmployeesRepository repository,
            PeopleController peopleController,
            PeopleRepository peopleRepository) : base(repository)
        {
            _repository = repository;
            _peopleRepository = peopleRepository;
            _peopleController = peopleController;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            var employees = await _repository.GetEmployees();

            return employees.Select(e => EmployeeToDTO(e));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _repository.GetEmployee(id);

            if (employee == null)
            {
                return NotFound();
            }

            return EmployeeToDTO(employee);
        }

        // PUT: api/Employees/5
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(EmployeeDTO employeeDTO)
        {
            var personId = _peopleRepository.GetPersonId(employeeDTO.Email);
            
            var personDTO = new PersonDTO
            {
                Id = personId,
                Email = employeeDTO.Email,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Addresses = employeeDTO.Addresses
            };

            await _peopleController.UpdatePerson(personDTO);

            var employee = await _repository.GetEmployee(employeeDTO.Id);

            employee.Position = employeeDTO.Position;
            employee.Salary = employeeDTO.Salary;

            return await _repository.Update(employee);
        }

        // POST: api/Employees
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employeeDTO)
        {
            var person = new Person
            {
                Email = employeeDTO.Email,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName
            };

            var addPersonResult = await _peopleRepository.Add(person);

            var result = addPersonResult.Result;

            var statusCode = GetHttpStatusCode(result).ToString();

            if (statusCode == "OK")
            {
                var employee = new Employee
                {
                    PersonId = _peopleRepository.GetPersonId(employeeDTO.Email),
                    Position = employeeDTO.Position,
                    Salary = employeeDTO.Salary
                };

                return await _repository.Add(employee);
            }
            else
            {
                var employee = new Employee
                {
                    PersonId = _peopleRepository.GetPersonId(employeeDTO.Email),
                    Position = employeeDTO.Position,
                    Salary = employeeDTO.Salary
                };

                return await _repository.Add(employee);
            }
        }

        private static EmployeeDTO EmployeeToDTO(Employee employee) =>
            new EmployeeDTO
            {
                Id = employee.Id,
                PersonId = employee.PersonId,
                Email = employee.Person.Email,
                FirstName = employee.Person.FirstName,
                LastName = employee.Person.LastName,
                Addresses = employee.Person.Addresses.Select(a => AddressesController.AddressToDTO(a)).ToList(),
                Position = employee.Position,
                Salary = employee.Salary
            };
        
        public static HttpStatusCode GetHttpStatusCode(IActionResult functionResult)
        {
            try
            {
                return (HttpStatusCode)functionResult
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(functionResult, null);
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
