using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class ClientsRepository : GenericRepository<Guid, Client, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public ClientsRepository(
            ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Client>> GetAll()
        {
            return await _context.Clients
                .Include(c => c.Person)
                .ThenInclude(p => p.Addresses)
                .ToListAsync();
        }

        public override async Task<Client> Get(Guid id)
        {
            return await _context.Clients
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Client> GetClient(string personId)
        {
            return await _context.Clients
                .Include(e => e.Person)
                .ThenInclude(p => p.Addresses)
                .SingleOrDefaultAsync(e => e.PersonId == personId);
        }        
    }
}
