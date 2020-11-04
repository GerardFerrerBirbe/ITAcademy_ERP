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

        public override async Task<List<Product>> GetAll()
        {
            return await _context.Products
               .Include(p => p.Category)
               .ToListAsync();
        }

        public override async Task<Product> Get(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.Id == id);         
        }

        public Guid GetProductId(string productName)
        {
            var productId = _context.Products.FirstOrDefault(x => x.Name == productName).Id;

            return productId;
        }
    }
}
