using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public interface IRepository<TProperty, TEntity> where TEntity : class, IEntity<TProperty>
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity> Get(Guid id);
        Task<IActionResult> Update(TEntity entity);
        Task<ActionResult> Add(TEntity entity);
        Task<TEntity> Delete(Guid id);
    }
}
