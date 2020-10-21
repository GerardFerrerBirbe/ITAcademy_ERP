using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Product : IEntity<Guid>
    {
        public Product()
        {
            OrderLines = new HashSet<OrderLine>();
        }
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public Guid ProductCategoryId { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
