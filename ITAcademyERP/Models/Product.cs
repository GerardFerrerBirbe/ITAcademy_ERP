using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }

        public virtual OrderLine OrderLines { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
