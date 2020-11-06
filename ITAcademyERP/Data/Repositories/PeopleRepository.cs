using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class PeopleRepository : GenericRepository<string, Person, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public PeopleRepository(
            ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Person> GetPerson(string id)
        {
            return await _context.People
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            return await _context.People
                .SingleOrDefaultAsync(p => p.Email == email);
        }

        public string GetPersonId(string email)
        {
            return _context.People
                .FirstOrDefault(p => p.Email == email)
                .Id;
        }

        public string GetPersonIdByName(string personName)
        {
            return _context.People
                            .FirstOrDefault(p => p.FullName == personName)
                            .Id;
        }

        public async Task<Person> Delete(string personId)
        {
            var person = await _context.People.FindAsync(personId);
            if (person == null)
            {
                return person;
            }

            _context.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }
    }
}
