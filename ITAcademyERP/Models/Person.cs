using ITAcademyERP.Data;
using Microsoft.AspNetCore.Identity;
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

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(30)]
        public string LastName { get; set; }
        
        [NotMapped]
        public string FullName
        {
            get
                {
                return FirstName + " " + LastName;
                }
        }

        
        public virtual ICollection<Address> Addresses { get; set; }
        
        public virtual Client Client { get; set; }
        
        public virtual Employee Employee { get; set; }
    }
}
