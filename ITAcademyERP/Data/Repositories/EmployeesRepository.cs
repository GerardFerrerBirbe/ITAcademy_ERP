using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class EmployeesRepository : GenericRepository<Guid, Employee, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public EmployeesRepository(
            ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Employee>> GetAll()
        {
            return await _context.Employees
                .Include(c => c.Person)
                .ThenInclude(p => p.Addresses)
                .ToListAsync();
        }

        public override async Task<Employee> Get(Guid id)
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
    }
}
