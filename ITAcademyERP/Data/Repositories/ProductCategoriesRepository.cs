using ITAcademyERP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class ProductCategoriesRepository : GenericRepository<Guid, ProductCategory, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;
        public ProductCategoriesRepository(ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }
        
        public Guid GetProductCategoryId(string name)
        {
            return _context.ProductCategories
                .FirstOrDefault(p => p.ProductCategoryName == name)
                .Id;
        }
    }
}
