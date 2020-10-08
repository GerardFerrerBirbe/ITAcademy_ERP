using ITAcademyERP.Data;
using System;
using System.Collections.Generic;

namespace ITAcademyERP.Models
{
    public class ProductDTO : IEntity
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public virtual ICollection<OrderLineDTO> OrderLines { get; set; }
    }
}
