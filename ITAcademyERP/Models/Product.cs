using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderLines = new HashSet<OrderLine>();
        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
