using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string ProductCategoryName { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
