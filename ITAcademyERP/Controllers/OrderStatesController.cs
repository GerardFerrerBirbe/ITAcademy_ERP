using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public OrderStatesController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/OrderStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderState>>> GetOrderState()
        {
            return await _context.OrderState.ToListAsync();
        }

        public int GetOrderStateId (string orderStateName)
        {
            var orderStateId = _context.OrderState
                            .FirstOrDefault(x => x.State == orderStateName)
                            .Id;

            return orderStateId;
        }
    }
}
