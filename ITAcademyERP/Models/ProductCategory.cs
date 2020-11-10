using ITAcademyERP.Data;
using Newtonsoft.Json;
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
        
        [StringLength(50, ErrorMessage ="El nom ha de tenir menys de 50 caràcters")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
