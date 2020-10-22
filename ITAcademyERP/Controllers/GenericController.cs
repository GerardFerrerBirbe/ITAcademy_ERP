﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ITAcademyERP.Data;
using ITAcademyERP.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericController<TProperty, TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity<TProperty>
        where TRepository : IRepository<TProperty, TEntity>
    {
        private readonly TRepository _repository;

        public GenericController(TRepository repository)
        {
            _repository = repository;
        }

        // GET: api/[controller]

        [HttpGet]
        [Route("generic/")]
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _repository.GetAll();
        }

        // GET: api/[controller]/5

        [HttpGet("generic/{id}")]
        public async Task<ActionResult<TEntity>> Get(Guid id)
        {
            var entity = await _repository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        // PUT: api/[controller]/5
        [HttpPut("generic/{id}")]
        public async Task<IActionResult> Put(TEntity entity)
        {
            return await _repository.Update(entity);
        }

        // POST: api/[controller]
        [Route("generic/")]
        [HttpPost]
        public async Task<ActionResult> Post(TEntity entity)
        {
            return await _repository.Add(entity);            
        }

        // DELETE: api/[controller]/5
        [HttpDelete("generic/{id}")]
        public async Task<ActionResult<TEntity>> Delete(Guid id)
        {
            var entity = await _repository.Delete(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        public static HttpStatusCode GetHttpStatusCode(IActionResult functionResult)
        {
            try
            {
                return (HttpStatusCode)functionResult
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(functionResult, null);
            }
            catch
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
