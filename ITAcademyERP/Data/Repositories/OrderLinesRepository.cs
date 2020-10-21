using ITAcademyERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class OrderLinesRepository : GenericRepository<Guid, OrderLine, ITAcademyERPContext>
    {
        public OrderLinesRepository(ITAcademyERPContext context) : base(context)
        {

        }


    }
}
