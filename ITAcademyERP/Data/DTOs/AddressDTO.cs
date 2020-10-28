using System;
using System.ComponentModel.DataAnnotations;

namespace ITAcademyERP.Data.DTOs
{
    public class AddressDTO
    {
        public string Id { get; set; }
        
        public string PersonId { get; set; }
        
        [Required(ErrorMessage = "Camp requerit")]
        [MaxLength(20, ErrorMessage = "El nom ha de tenir com a màxim 20 caràcters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Camp requerit")]
        public string Type { get; set; }
    }
}   
