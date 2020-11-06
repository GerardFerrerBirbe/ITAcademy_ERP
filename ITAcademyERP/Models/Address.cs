using ITAcademyERP.Data;
using ITAcademyERP.Data.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class Address : IEntity<Guid>
    {
        public Address()
        {
        }

        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey("PersonId")]
        public string PersonId { get; set; }

        //[Required(ErrorMessage = "Camp requerit")]
        [MaxLength(20, ErrorMessage = "El nom ha de tenir com a màxim 20 caràcters")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Camp requerit")]
        [Column("Type", TypeName = "int")]
        public AddressType Type { get; set; }       

        [JsonIgnore]
        public virtual Person Person { get; set; }
    }
}
