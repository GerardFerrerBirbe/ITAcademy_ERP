using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using SQLitePCL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;
        private readonly PeopleController _peopleController;

        public EmployeesController(
            ITAcademyERPContext context,
            PeopleController peopleController)
        {
            _context = context;
            _peopleController = peopleController;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {            
            return await _context.Employee
                .Include(e => e.Person)
                .ThenInclude(p => p.PersonalAddress)
                .Select(e => EmployeeToDTO(e))
                .ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {              
            var employee = await _context.Employee
                .Include(e => e.Person)
                .ThenInclude(p => p.PersonalAddress)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                return BadRequest();
            }

            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Position = employeeDTO.Position;
            employee.Salary = employeeDTO.Salary;

            _context.Entry(employee).State = EntityState.Modified;

            var personDTO = new PersonDTO
            {
                Email = employeeDTO.Email,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Address = employeeDTO.Address
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
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employeeDTO)
        {
            var addressExists = _context.Address.FirstOrDefault(a => a.AddressName == employeeDTO.Address);

            if (addressExists == default)
            {
                var address = new Address
                {
                    AddressName = employeeDTO.Address
                };

                _context.Address.Add(address);

                _context.SaveChanges();
            }

            var addressId = _context.Address.FirstOrDefault(a => a.AddressName == employeeDTO.Address).Id;            

            var personExists = _context.Person.FirstOrDefault(p => p.Email == employeeDTO.Email);

            if (personExists == default)
            {
                var person = new Person
                {
                    Email = employeeDTO.Email,
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    PersonalAddressId = addressId
                };

                _context.Person.Add(person);

                _context.SaveChanges();
            }            

            var personId = _context.Person.FirstOrDefault(p => p.Email == employeeDTO.Email).Id;

            var employee = new Employee
            {
                PersonId = personId,
                Position = employeeDTO.Position,
                Salary = employeeDTO.Salary
            };

            _context.Employee.Add(employee);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, EmployeeToDTO(employee));
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

        private static EmployeeDTO EmployeeToDTO(Employee employee) =>
            new EmployeeDTO
            {
                Id = employee.Id,
                Email = employee.Person.Email,
                FirstName = employee.Person.FirstName,
                LastName = employee.Person.LastName,
                Address = employee.Person.PersonalAddress.AddressName,
                Position = employee.Position,
                Salary = employee.Salary
            };

        public int GetEmployeeId (string employeeName)
        {
            var personEmployeeId = _context.Person
                           .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == employeeName)
                           .Id;

            var employeeId = _context.Employee
                            .FirstOrDefault(x => x.PersonId == personEmployeeId)
                            .Id;

            return employeeId;
        }
    }
}
