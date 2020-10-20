using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class OrderHeadersRepository : GenericRepository<OrderHeader, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public OrderHeadersRepository(
            ITAcademyERPContext context
            ) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderHeader>> GetOrderHeaders()
        {
            return await _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .ToListAsync();
        }

        public async Task<List<OrderHeader>> GetOrderHeadersByEmployee(int employeeId)
        {
            return await _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .Where(o => o.EmployeeId == employeeId)
                    .ToListAsync();
        }

        public async Task<List<OrderHeader>> GetOrderHeadersByClient(int clientId)
        {
            return await _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .Where(o => o.ClientId == clientId)
                    .ToListAsync();
        }

        public async Task<OrderHeader> GetOrderHeader(int id)
        {
            return await _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .SingleOrDefaultAsync(o => o.Id == id);
        }
    }
}
