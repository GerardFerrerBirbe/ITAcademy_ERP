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
    public class ProductsRepository : GenericRepository<Guid, Product, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;
        public ProductsRepository(
            ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products
               .Include(p => p.ProductCategory)
               .ToListAsync();
        }

        public async Task<Product> GetProduct(Guid id)
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .SingleOrDefaultAsync(p => p.Id == id);           
        }

        public Guid GetProductId(string productName)
        {
            var productId = _context.Products.FirstOrDefault(x => x.ProductName == productName).Id;

            return productId;
        }
    }
}
