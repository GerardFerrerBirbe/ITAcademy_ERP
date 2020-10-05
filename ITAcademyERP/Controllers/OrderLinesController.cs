using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ITAcademyERP.Data;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Employee")]
    [ApiController]
    public class OrderLinesController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;
        private readonly ProductsController _productsController;


        public OrderLinesController(
            ITAcademyERPContext context,
            ProductsController productsController)
        {
            _context = context;
            _productsController = productsController;
        }

        // GET: api/OrderLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderLineDTO>>> GetOrderLines()
        {
            return await _context.OrderLines
                .Select(p => OrderLineToDTO(p))
                .ToListAsync();
        }

        // POST: api/OrderLines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OrderLine>> AddOrderLine(OrderLineDTO orderLineDTO)
        {
            var orderLine = new OrderLine
            {
                Id = orderLineDTO.Id,
                OrderHeaderId = orderLineDTO.OrderHeaderId,
                ProductId = _productsController.GetProductId(orderLineDTO.ProductName),
                UnitPrice = orderLineDTO.UnitPrice,
                Vat = orderLineDTO.Vat,
                Quantity = orderLineDTO.Quantity
            };

            _context.OrderLines.Add(orderLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderLine", new { id = orderLine.Id }, OrderLineToDTO(orderLine));
        }

        // DELETE: api/OrderLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderLine>> DeleteOrderLine(int id)
        {
            var orderLine = await _context.OrderLines.FindAsync(id);

            if (orderLine == null)
            {
                return NotFound();
            }

            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();

            return orderLine;
        }

        private static OrderLineDTO OrderLineToDTO(OrderLine orderLine)
        {

            var orderLineDTO = new OrderLineDTO
            {
                Id = orderLine.Id,
                OrderHeaderId = orderLine.OrderHeaderId,
                ProductName = orderLine.Product.ProductName,
                UnitPrice = orderLine.UnitPrice,
                Vat = orderLine.Vat,
                Quantity = orderLine.Quantity
            };

            return orderLineDTO;
        }
    }
}
