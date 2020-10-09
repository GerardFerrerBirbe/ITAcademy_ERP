using ITAcademyERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITAcademyERP.Data.Repositories
{
    public class OrderStatesRepository : GenericRepository<OrderState, ITAcademyERPContext>
    {
        private readonly ITAcademyERPContext _context;
        public OrderStatesRepository(ITAcademyERPContext context) : base(context)
        {
            _context = context;
        }

        public int GetOrderStateId(string name)
        {
            return _context.OrderStates
                .FirstOrDefault(o => o.State == name)
                .Id;
        }
    }
}
