using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class ProductCategory : IEntity<Guid>
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }
        
        public Guid Id { get; set; }        
        
        public string ProductCategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
