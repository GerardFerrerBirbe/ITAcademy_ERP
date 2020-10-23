using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class Product : IEntity<Guid>
    {
        public Product()
        {
            OrderLines = new HashSet<OrderLine>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [ForeignKey("ProductCategoryId")]
        public Guid ProductCategoryId { get; set; }

        
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
