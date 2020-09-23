using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHeadersController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public OrderHeadersController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/OrderHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHeaderDTO>>> GetOrderHeaders()
        {
            var output = _context.OrderHeader
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
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
                orderHeader = await _context.OrderHeader
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
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
                orderHeader = await _context.OrderHeader
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
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

            var orderHeader = await _context.OrderHeader.FindAsync(id);

            if (orderHeader == null)
            {
                return NotFound();
            }

            var deliveryAddressId = _context.Address
                            .FirstOrDefault(x => x.AddressName == orderHeaderDTO.Address)
                            .Id;

            var personClientId = _context.Person
                            .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == orderHeaderDTO.Client)
                            .Id;

            var clientId = _context.Client
                            .FirstOrDefault(x => x.PersonId == personClientId)
                            .Id;

            var personEmployeeId = _context.Person
                            .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == orderHeaderDTO.Employee)
                            .Id;

            var employeeId = _context.Employee
                            .FirstOrDefault(x => x.PersonId == personEmployeeId)
                            .Id;

            var orderStateId = _context.OrderState
                            .FirstOrDefault(x => x.State == orderHeaderDTO.OrderState)
                            .Id;

            var orderPriorityId = _context.OrderPriority
                            .FirstOrDefault(x => x.Priority == orderHeaderDTO.OrderPriority)
                            .Id;           

            orderHeader.OrderNumber = orderHeaderDTO.OrderNumber;
            orderHeader.DeliveryAddressId = deliveryAddressId;
            orderHeader.ClientId = clientId;
            orderHeader.EmployeeId = employeeId;
            orderHeader.OrderStateId = orderStateId;
            orderHeader.OrderPriorityId = orderPriorityId;
            orderHeader.CreationDate = Convert.ToDateTime(orderHeaderDTO.CreationDate);
            orderHeader.AssignToEmployeeDate = Convert.ToDateTime(orderHeaderDTO.AssignToEmployeeDate);
            orderHeader.FinalisationDate = Convert.ToDateTime(orderHeaderDTO.FinalisationDate);

            _context.Entry(orderHeader).State = EntityState.Modified;                   

            try
            {
                await CreateOrEditOrderLines(orderHeaderDTO.OrderLines);
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

        private async Task CreateOrEditOrderLines(ICollection<OrderLineDTO> orderLinesDTO)
        {
            ICollection<OrderLineDTO> orderLinesToCreate = orderLinesDTO.Where(x => x.Id == 0).ToList();
            ICollection<OrderLineDTO> orderLinesToEdit = orderLinesDTO.Where(x => x.Id != 0).ToList();

            if (orderLinesToCreate.Any())
            {               
                foreach (var orderLineDTO in orderLinesToCreate)
                {
                    if (orderLineDTO.ProductName == "")
                        return;

                    var productId = _context.Product.FirstOrDefault(x => x.ProductName == orderLineDTO.ProductName).Id;

                    var orderLine = new OrderLine
                    {
                        Id = orderLineDTO.Id,
                        OrderHeaderId = orderLineDTO.OrderHeaderId,
                        ProductId = productId,
                        UnitPrice = orderLineDTO.UnitPrice,
                        Vat = orderLineDTO.Vat,
                        Quantity = orderLineDTO.Quantity
                    };

                    _context.OrderLine.Add(orderLine);
                    await _context.SaveChangesAsync();

                    CreatedAtAction("GetOrderLine", new { id = orderLine.Id }, OrderLineToDTO(orderLine));
                }                
            }

            if (orderLinesToEdit.Any())
            {
                foreach (var orderLineDTO in orderLinesToEdit)
                {
                    var orderLine = _context.OrderLine.FirstOrDefault(x => x.Id == orderLineDTO.Id);

                    var productId = _context.Product.FirstOrDefault(x => x.ProductName == orderLineDTO.ProductName).Id;

                    orderLine.OrderHeaderId = orderLineDTO.OrderHeaderId;
                    orderLine.ProductId = productId;
                    orderLine.UnitPrice = orderLineDTO.UnitPrice;
                    orderLine.Vat = orderLineDTO.Vat;
                    orderLine.Quantity = orderLineDTO.Quantity;
                    
                    _context.Entry(orderLine).State = EntityState.Modified;
                }
            }
        }

        // POST: api/OrderHeaders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OrderHeader>> PostOrderHeader(OrderHeaderDTO orderHeaderDTO)
        {
            var deliveryAddressId = _context.Address
                            .FirstOrDefault(x => x.AddressName == orderHeaderDTO.Address)
                            .Id;

            var personClientId = _context.Person
                            .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == orderHeaderDTO.Client)
                            .Id;

            var clientId = _context.Client
                            .FirstOrDefault(x => x.PersonId == personClientId)
                            .Id;

            var personEmployeeId = _context.Person
                            .FirstOrDefault(x => x.FirstName + ' ' + x.LastName == orderHeaderDTO.Employee)
                            .Id;

            var employeeId = _context.Employee
                            .FirstOrDefault(x => x.PersonId == personEmployeeId)
                            .Id;

            var orderStateId = _context.OrderState
                            .FirstOrDefault(x => x.State == orderHeaderDTO.OrderState)
                            .Id;

            var orderPriorityId = _context.OrderPriority
                            .FirstOrDefault(x => x.Priority == orderHeaderDTO.OrderPriority)
                            .Id;
            
            var orderHeader = new OrderHeader
            {
                OrderNumber = orderHeaderDTO.OrderNumber,
                DeliveryAddressId = deliveryAddressId,
                ClientId = clientId,
                EmployeeId = employeeId,
                OrderStateId = orderStateId,
                OrderPriorityId = orderPriorityId,
                CreationDate = Convert.ToDateTime(orderHeaderDTO.CreationDate),
                AssignToEmployeeDate = Convert.ToDateTime(orderHeaderDTO.AssignToEmployeeDate),
                FinalisationDate = Convert.ToDateTime(orderHeaderDTO.FinalisationDate)
            };

            _context.OrderHeader.Add(orderHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderHeader", new { id = orderHeader.Id }, OrderHeaderToDTO(orderHeader));
        }

        // DELETE: api/OrderHeaders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderHeader>> DeleteOrderHeader(int id)
        {
            var orderHeader = await _context.OrderHeader.FindAsync(id);
            if (orderHeader == null)
            {
                return NotFound();
            }

            _context.OrderHeader.Remove(orderHeader);
            await _context.SaveChangesAsync();

            return orderHeader;
        }

        private bool OrderHeaderExists(int id)
        {
            return _context.OrderHeader.Any(e => e.Id == id);
        }

        public static OrderHeaderDTO OrderHeaderToDTO(OrderHeader orderHeader) {

            var orderHeaderDTO = new OrderHeaderDTO
            {
                Id = orderHeader.Id,
                OrderNumber = orderHeader.OrderNumber,
                Address = orderHeader.DeliveryAddress.AddressName,
                Client = orderHeader.Client.Person.FirstName + ' ' + orderHeader.Client.Person.LastName,
                Employee = orderHeader.Employee.Person.FirstName + ' ' + orderHeader.Employee.Person.LastName,
                OrderState = orderHeader.OrderState.State,
                OrderPriority = orderHeader.OrderPriority.Priority,
                CreationDate = orderHeader.CreationDate,
                AssignToEmployeeDate = orderHeader.AssignToEmployeeDate,
                FinalisationDate = orderHeader.FinalisationDate,
                OrderLines = orderHeader.OrderLines.Select(o => OrderLineToDTO(o)).ToList()
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
