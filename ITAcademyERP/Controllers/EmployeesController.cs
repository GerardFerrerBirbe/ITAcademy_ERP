using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using SQLitePCL;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public EmployeesController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {            
            return await _context.Employee
                .Include(e => e.Person)
                .Select(e => EmployeeToDTO(e))
                .ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {              
            var employee = await _context.Employee
                .Include(e => e.Person)
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

            if(employee == null)
            {
                return NotFound();
            }

            var personId = _context.Person
                            .FirstOrDefault(p => p.FirstName == employeeDTO.FirstName)
                            .Id;

            var person = await _context.Person.FindAsync(personId);

            if (person == null)
            {
                return NotFound();
            }
            
            employee.Position = employeeDTO.Position;
            employee.Salary = employeeDTO.Salary;
            employee.UserName = employeeDTO.UserName;
            employee.Password = employeeDTO.Password;
            
            person.FirstName = employeeDTO.FirstName;
            person.LastName = employeeDTO.LastName;

            _context.Entry(employee).State = EntityState.Modified;
            _context.Entry(person).State = EntityState.Modified;

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
            var personId = _context.Person
                            .FirstOrDefault(p => p.FirstName == employeeDTO.FirstName)
                            .Id;

            var employee = new Employee
            {
                PersonId = personId,
                Position = employeeDTO.Position,
                Salary = employeeDTO.Salary,
                UserName = employeeDTO.UserName,
                Password = employeeDTO.Password
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
                FirstName = employee.Person.FirstName,
                LastName = employee.Person.LastName,
                Position = employee.Position,
                Salary = employee.Salary,
                UserName = employee.UserName,
                Password = employee.Password
            };
    }
}
