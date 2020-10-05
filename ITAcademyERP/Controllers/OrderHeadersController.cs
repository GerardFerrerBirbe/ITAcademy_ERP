using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using ITAcademyERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class OrderHeadersController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;
        private readonly ClientsController _clientsController;
        private readonly EmployeesController _employeesController;
        private readonly OrderPrioritiesController _orderPrioritiesController;
        private readonly OrderStatesController _orderStatesController;
        private readonly ProductsController _productsController;

        public OrderHeadersController(
            ITAcademyERPContext context,
            ClientsController clientsController,
            EmployeesController employeesController,
            OrderPrioritiesController orderPrioritiesController,
            OrderStatesController orderStatesController,
            ProductsController productsController)
        {
            _context = context;
            _clientsController = clientsController;
            _employeesController = employeesController;
            _orderPrioritiesController = orderPrioritiesController;
            _orderStatesController = orderStatesController;
            _productsController = productsController;
        }

        // GET: api/OrderHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeaderDTO>>> GetOrderHeaders()
        {
            var output = _context.OrderHeaders                    
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .Select(o => OrderHeaderToDTO(o))                    
                    .ToListAsync();        

            return await output;
        }

        [Route("Employee")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeaderDTO>>> GetOrderHeadersByEmployee(int employeeId)
        {
            var output = _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .Where(o => o.EmployeeId == employeeId)
                    .Select(o => OrderHeaderToDTO(o))
                    .ToListAsync();

            return await output;
        }

        [Route("Client")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeaderDTO>>> GetOrderHeadersByClient(int clientId)
        {
            var output = _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .Where(o => o.ClientId == clientId)
                    .Select(o => OrderHeaderToDTO(o))
                    .ToListAsync();

            return await output;
        }

        //GET: api/OrderHeaders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderHeader([FromRoute] int id, bool includeOrderLines = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OrderHeader orderHeader;

            if (includeOrderLines)
            {
                orderHeader = await _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .Include(o => o.OrderLines)
                    .ThenInclude(o => o.Product)
                    .SingleOrDefaultAsync(o => o.Id == id);
            }
            else
            {
                orderHeader = await _context.OrderHeaders
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .ThenInclude(p => p.Addresses)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .SingleOrDefaultAsync(o => o.Id == id);
            }

            if (orderHeader == null)
            {
                return NotFound();
            }

            return Ok(OrderHeaderToDTO(orderHeader));
        }

        // PUT: api/OrderHeaders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderHeader(int id, OrderHeaderDTO orderHeaderDTO)
        {
            if (id != orderHeaderDTO.Id)
            {
                return BadRequest();
            }

            var orderHeader = await _context.OrderHeaders.FindAsync(id);

            if (orderHeader == null)
            {
                return NotFound();
            }            
            
            if (orderHeaderDTO.OrderState == "Complet" && _orderStatesController.GetOrderStateId(orderHeaderDTO.OrderState) != orderHeader.OrderStateId)
            {
                orderHeader.FinalisationDate = DateTime.Now;
            }

            if (_employeesController.GetEmployeeId(orderHeaderDTO.Employee) != orderHeader.EmployeeId)
            {
                orderHeader.AssignToEmployeeDate = DateTime.Now;
            }

            orderHeader.OrderNumber = orderHeaderDTO.OrderNumber;
            orderHeader.ClientId = _clientsController.GetClientId(orderHeaderDTO.Client);
            orderHeader.EmployeeId = _employeesController.GetEmployeeId(orderHeaderDTO.Employee);
            orderHeader.OrderStateId = _orderStatesController.GetOrderStateId(orderHeaderDTO.OrderState);
            orderHeader.OrderPriorityId = _orderPrioritiesController.GetOrderPriorityId(orderHeaderDTO.OrderPriority);

            _context.Entry(orderHeader).State = EntityState.Modified;                   

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderHeaderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }       

        // POST: api/OrderHeaders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OrderHeader>> PostOrderHeader(OrderHeaderDTO orderHeaderDTO)
        {            
            
            var orderHeader = new OrderHeader
            {
                OrderNumber = orderHeaderDTO.OrderNumber,
                ClientId = _clientsController.GetClientId(orderHeaderDTO.Client),
                EmployeeId = _employeesController.GetEmployeeId(orderHeaderDTO.Employee),
                OrderStateId = _orderStatesController.GetOrderStateId(orderHeaderDTO.OrderState),
                OrderPriorityId = _orderPrioritiesController.GetOrderPriorityId(orderHeaderDTO.OrderPriority),
                CreationDate = DateTime.Now,
                AssignToEmployeeDate = DateTime.Now
            };

            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderHeader", new { id = orderHeader.Id }, OrderHeaderToDTO(orderHeader));
        }

        //DELETE: api/OrderHeaders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderHeader>> DeleteOrderHeader(int id)
        {
            var orderHeader = await _context.OrderHeaders.FindAsync(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            _context.OrderHeaders.Remove(orderHeader);
            await _context.SaveChangesAsync();

            return orderHeader;
        }

        private bool OrderHeaderExists(int id)
        {
            return _context.OrderHeaders.Any(e => e.Id == id);
        }

        public static OrderHeaderDTO OrderHeaderToDTO(OrderHeader orderHeader) {

            var orderLinesDTO = new List<OrderLineDTO>();

            foreach (var orderLine in orderHeader.OrderLines)
            {
                var orderLineDTO = OrderLineToDTO(orderLine);
                orderLinesDTO.Add(orderLineDTO);
            }
            
            var orderHeaderDTO = new OrderHeaderDTO
            {
                Id = orderHeader.Id,
                OrderNumber = orderHeader.OrderNumber,
                Address = orderHeader.Client.Person.Addresses.FirstOrDefault(a => a.Type == "Delivery")?.Name,
                Client = orderHeader.Client.Person.FirstName + ' ' + orderHeader.Client.Person.LastName,
                Employee = orderHeader.Employee.Person.FirstName + ' ' + orderHeader.Employee.Person.LastName,
                OrderState = orderHeader.OrderState.State,
                OrderPriority = orderHeader.OrderPriority.Priority,
                CreationDate = orderHeader.CreationDate,
                AssignToEmployeeDate = orderHeader.AssignToEmployeeDate,
                FinalisationDate = orderHeader.FinalisationDate == null ? null : orderHeader.FinalisationDate,
                OrderLines = orderLinesDTO
            };

            return orderHeaderDTO;
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
