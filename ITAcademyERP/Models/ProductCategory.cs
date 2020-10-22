using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITAcademyERP.Models
{
    public partial class ProductCategory : IEntity<Guid>
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }
        
        [Key]
        public Guid Id { get; set; }        
        [Required, StringLength(50)]
        public string ProductCategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
