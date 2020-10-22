using ITAcademyERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public abstract class GenericRepository<TProperty, TEntity, TContext> : ControllerBase, IRepository<TProperty, TEntity>
        where TEntity : class, IEntity<TProperty>
        where TContext : IdentityDbContext<Person>
    {
        private readonly TContext _context;
        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> Get(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IActionResult> Update(TEntity entity)
        {            
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.InnerException.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        public async Task<ActionResult> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.InnerException.Message);
                return BadRequest(ModelState);
            }

            return Ok(entity);
        }

        public async Task<TEntity> Delete(Guid id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
