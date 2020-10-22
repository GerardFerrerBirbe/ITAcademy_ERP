using ITAcademyERP.Data;
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
        [Required, MaxLength(20, ErrorMessage ="El nom ha de tenir com a màxim 20 caràcters")]
        public string Name { get; set; }
        [Required, Column("Type", TypeName = "int")]
        public AddressType Type { get; set; }
        [Required, ForeignKey("PersonId")]
        public string PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
