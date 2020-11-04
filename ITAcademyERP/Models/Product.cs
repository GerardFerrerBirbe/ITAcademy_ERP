using ITAcademyERP.Data;
using Newtonsoft.Json;
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

        [Required(ErrorMessage = "Camp requerit")]
        [StringLength(50, ErrorMessage = "El producte ha de tenir menys de 50 caràcters")]
        public string Name { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Camp requerit")]
        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }


        [JsonIgnore]
        public virtual ICollection<OrderLine> OrderLines { get; set; }

        public virtual ProductCategory Category { get; set; }
    }
}
