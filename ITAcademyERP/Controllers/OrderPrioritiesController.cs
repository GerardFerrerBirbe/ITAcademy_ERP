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
    public class OrderPrioritiesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public OrderPrioritiesController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/OrderPriorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderPriority>>> GetOrderPriority()
        {
            return await _context.OrderPriority.ToListAsync();
        }
    }
}
