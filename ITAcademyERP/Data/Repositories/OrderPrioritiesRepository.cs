using ITAcademyERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class OrderPrioritiesRepository : GenericRepository<OrderPriority, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;
        public OrderPrioritiesRepository(ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }

        public int GetOrderPriorityId(string name)
        {
            return _context.OrderPriorities
                .FirstOrDefault(o => o.Priority == name)
                .Id;
        }
    }
}
