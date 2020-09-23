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
    public class OHByClientController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public OHByClientController(ITAcademyERPContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeaderDTO>>> GetOrderHeadersByClient(int clientId)
        {
            var output = _context.OrderHeader
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .Where(o => o.ClientId == clientId)
                    .Select(o => OrderHeadersController.OrderHeaderToDTO(o))
                    .ToListAsync();

            return await output;
        }
    }
}
