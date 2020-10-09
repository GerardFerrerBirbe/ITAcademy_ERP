using ITAcademyERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class OrderLinesRepository : GenericRepository<OrderLine, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;

        public OrderLinesRepository(ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }


    }
}
