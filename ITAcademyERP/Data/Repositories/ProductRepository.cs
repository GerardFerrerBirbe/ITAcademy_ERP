using ITAcademyERP.Controllers;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;
        private readonly ProductCategoriesRepository _productCategoriesRepository;
        public ProductRepository(
            ITAcademyERPContext context,
            ProductCategoriesRepository productCategoriesRepository) : base(context)
        {
            _context = context;
            _productCategoriesRepository = productCategoriesRepository;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products
               .Include(p => p.ProductCategory)
               .ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .SingleOrDefaultAsync(p => p.Id == id);           
        }

        public async Task<IActionResult> UpdateProduct(ProductDTO productDTO)
        {
            var product = await GetProduct(productDTO.Id);       

            product.ProductName = productDTO.ProductName;
            product.ProductCategoryId = _productCategoriesRepository.GetProductCategoryId(productDTO.ProductCategoryName);

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.InnerException.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        public async Task<ActionResult<Product>> AddProduct(ProductDTO productDTO)
        {
            var product = new Product
            {
                ProductName = productDTO.ProductName,
                ProductCategoryId = _productCategoriesRepository.GetProductCategoryId(productDTO.ProductCategoryName)
            };

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
                CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.InnerException.Message);
                return BadRequest(ModelState);
            }

            return Ok(product);
        }
    }
}
