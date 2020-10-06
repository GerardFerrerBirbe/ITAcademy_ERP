using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
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
            return await _context.OrderPriorities.ToListAsync();
        }

        public int GetOrderPriorityId(string orderPriorityName)
        {
            var orderPriorityId = _context.OrderPriorities
                            .FirstOrDefault(x => x.Priority == orderPriorityName)
                            .Id;

            return orderPriorityId;
        }
    }
}
