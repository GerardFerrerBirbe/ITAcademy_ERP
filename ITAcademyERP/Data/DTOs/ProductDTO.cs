using ITAcademyERP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Data.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Camp requerit")]
        [StringLength(50, ErrorMessage = "El producte ha de tenir menys de 50 caràcters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Camp requerit")]        
        public string ProductCategoryName { get; set; }

        public string YearMonth { get; set; }

        public double TotalSales { get; set; }

            


        public virtual ICollection<OrderLineDTO> OrderLines { get; set; }
    }
}
