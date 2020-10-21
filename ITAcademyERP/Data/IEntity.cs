using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data
{
    public interface IEntity<TProperty>
    {
         TProperty Id { get; set; }
    }
}
