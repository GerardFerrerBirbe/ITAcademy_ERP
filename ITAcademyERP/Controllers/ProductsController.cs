using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using ITAcademyERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Employee")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public ProductsController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct()
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .Select(p => ProductToDTO(p))
                .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return ProductToDTO(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }            

            var productCategoryId = _context.ProductCategories
                            .FirstOrDefault(p => p.ProductCategoryName == productDTO.ProductCategoryName)
                            .Id;

            product.ProductName = productDTO.ProductName;
            product.ProductCategoryId = productCategoryId;
            
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            var productCategoryId = _context.ProductCategories
                            .FirstOrDefault(p => p.ProductCategoryName == productDTO.ProductCategoryName)
                            .Id;

            var product = new Product
            {
                ProductName = productDTO.ProductName,
                ProductCategoryId = productCategoryId
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, ProductToDTO(product));
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductCategoryName = product.ProductCategory.ProductCategoryName
            };

        public int GetProductId (string productName)
        {
            var productId = _context.Products.FirstOrDefault(x => x.ProductName == productName).Id;

            return productId;
        }
    }
}
