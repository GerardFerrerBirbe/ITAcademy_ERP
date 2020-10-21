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
        private readonly PeopleRepository _peopleRepository;

        public ClientsRepository(
            ITAcademyERPContext context,
            PeopleRepository peopleRepository) : base(context)
        {
            _context = context;
            _peopleRepository = peopleRepository;
        }

        public async Task<List<Client>> GetClients()
        {
            return await _context.Clients
                .Include(c => c.Person)
                .ThenInclude(p => p.Addresses)
                .ToListAsync();
        }

        public async Task<Client> GetClient(Guid id)
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

        public Guid GetClientId(string clientName)
        {
            var personId = _peopleRepository.GetPersonIdByName(clientName);

            return _context.Clients
                    .FirstOrDefault(x => x.PersonId == personId)
                    .Id;
        }        
    }
}
