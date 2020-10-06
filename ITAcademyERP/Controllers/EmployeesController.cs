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

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;
        private readonly PeopleController _peopleController;
        private readonly UserManager<Person> _userManager;

        public EmployeesController(
            ITAcademyERPContext context,
            PeopleController peopleController,
            UserManager<Person> userManager)
        {
            _context = context;
            _peopleController = peopleController;
            _userManager = userManager;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {            
            return await _context.Employees
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .Select(e => EmployeeToDTO(e))
                .ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {              
            var employee = await _context.Employees
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .SingleOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return EmployeeToDTO(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            var employee = await _context.Employees
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .SingleOrDefaultAsync(e => e.Id == id);

            if (employeeDTO.Email != employee.Person.Email && _peopleController.EmailExists(employeeDTO.Email))
            {
                ModelState.AddModelError(string.Empty, "Email ja existent");
                return BadRequest(ModelState);
            }            

            employee.Position = employeeDTO.Position;
            employee.Salary = employeeDTO.Salary;

            _context.Entry(employee).State = EntityState.Modified;

            var personDTO = new PersonDTO
            {
                Email = employeeDTO.Email,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Addresses = employeeDTO.Addresses
            };

            await _peopleController.UpdatePerson(employee.PersonId, personDTO);           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employeeDTO)
        {                            
            if (!_peopleController.EmailExists(employeeDTO.Email))
            {
                var person = new Person
                {
                    Email = employeeDTO.Email,
                    UserName = employeeDTO.Email,
                    NormalizedUserName = employeeDTO.Email.ToUpper(),
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName
                };

                _context.People.Add(person);

                _context.SaveChanges();
            }

            var employeeExists = _context.Employees.FirstOrDefault(e => e.Person.Email == employeeDTO.Email);

            if (employeeExists != default)
            {
                ModelState.AddModelError(string.Empty, "Empleat ja existent");
                return BadRequest(ModelState);
            }
            else
            {
                var personId = _context.People.FirstOrDefault(p => p.Email == employeeDTO.Email).Id;

                var employee = new Employee
                {
                    PersonId = personId,
                    Position = employeeDTO.Position,
                    Salary = employeeDTO.Salary
                };

                _context.Employees.Add(employee);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, EmployeeToDTO(employee));
            }
           
        }

        // DELETE: api/Employees/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
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

        public int GetEmployeeId (string employeeName)
        {
            var personEmployeeId = _context.People
                           .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == employeeName)
                           .Id;

            var employeeId = _context.Employees
                            .FirstOrDefault(x => x.PersonId == personEmployeeId)
                            .Id;

            return employeeId;
        }
    }
}
