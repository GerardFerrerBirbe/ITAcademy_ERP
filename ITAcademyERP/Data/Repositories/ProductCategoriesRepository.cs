using ITAcademyERP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class ProductCategoriesRepository : GenericRepository<ProductCategory, ITAcademyERPContext>
    {
        public ProductCategoriesRepository(ITAcademyERPContext context) : base(context)
        {
            
        }        
    }
}
