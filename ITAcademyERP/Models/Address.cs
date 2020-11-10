using ITAcademyERP.Data;
using ITAcademyERP.Data.Resources;
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

        [MaxLength(20, ErrorMessage = "El nom ha de tenir com a màxim 20 caràcters")]
        public string Name { get; set; }

        [Column("Type", TypeName = "int")]
        public EAddressType Type { get; set; }       

        [JsonIgnore]
        public virtual Person Person { get; set; }
    }
}
