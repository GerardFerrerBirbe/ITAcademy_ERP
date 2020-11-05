using ITAcademyERP.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAcademyERP.Models
{
    public partial class Person: IdentityUser, IEntity<string>
    {
        public Person()
        {
            Addresses = new HashSet<Address>();
        }

        [Required(ErrorMessage = "Camp requerit")]
        [StringLength(30, ErrorMessage = "El nom ha de tenir menys de 30 caràcters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Camp requerit")]
        [StringLength(40, ErrorMessage = "El cognom ha de tenir menys de 40 caràcters")]
        public string LastName { get; set; }
        
        [NotMapped]
        public string FullName
        {
            get
                {
                return FirstName + " " + LastName;
                }
        }

        #region overrides

        [Required(ErrorMessage = "Camp requerit")]
        [StringLength(30, ErrorMessage = "L'email ha de tenir menys de 30 caràcters")]
        public override string Email { get; set; }
        
        [JsonIgnore]
        public override string UserName { get; set; }

        [JsonIgnore]
        public override string NormalizedUserName { get; set; }

        [JsonIgnore]
        public override string NormalizedEmail { get; set; }

        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }

        [JsonIgnore]
        public override string PhoneNumber { get; set; }

        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        
        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        #endregion


        public virtual ICollection<Address> Addresses { get; set; }
        
        [JsonIgnore]
        public virtual Client Client { get; set; }
        
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
    }
}
