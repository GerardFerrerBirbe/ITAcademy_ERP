using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class EmployeesRepository : GenericRepository<Employee, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;
        private readonly PeopleRepository _peopleRepository;

        public EmployeesRepository(
            ITAcademyERPContext context,
            PeopleRepository peopleRepository) : base(context)
        {
            _context = context;
            _peopleRepository = peopleRepository;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employees
                .Include(c => c.Person)
                .ThenInclude(p => p.Addresses)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _context.Employees
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> GetEmployee(string personId)
        {
            return await _context.Employees
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .SingleOrDefaultAsync(e => e.PersonId == personId);
        }

        public int GetEmployeeId(string employeeName)
        {
            var personId = _peopleRepository.GetPersonIdByName(employeeName);

            return _context.Employees
                    .FirstOrDefault(x => x.PersonId == personId)
                    .Id;
        }
    }
}
