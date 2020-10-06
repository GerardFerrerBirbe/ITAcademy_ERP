using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using ITAcademyERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public ProductCategoriesController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategory()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {            
            var productCategory = await _context.ProductCategories
                    .SingleOrDefaultAsync(p => p.Id == id);
            
            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.Id }, productCategory);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductCategory>> DeleteProductCategory(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();

            return productCategory;
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }
    }
}
