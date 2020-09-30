using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Employee")]
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
            return await _context.OrderStates.ToListAsync();
        }

        public int GetOrderStateId (string orderStateName)
        {
            var orderStateId = _context.OrderStates
                            .FirstOrDefault(x => x.State == orderStateName)
                            .Id;

            return orderStateId;
        }
    }
}
