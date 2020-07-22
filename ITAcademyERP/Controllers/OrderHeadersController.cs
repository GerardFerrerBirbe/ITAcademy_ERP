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
        public async Task<ActionResult<IEnumerable<OrderHeaderDTO>>> GetOrderHeader()
        {
            return await _context.OrderHeader
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Client)
                    .ThenInclude(c => c.Person)
                    .Include(o => o.Employee)
                    .ThenInclude(e => e.Person)
                    .Include(o => o.OrderState)
                    .Include(o => o.OrderPriority)
                    .Select(o => OrderHeaderToDTO(o))
                    .ToListAsync();
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

            orderHeader.OrderNumber = orderHeaderDTO.OrderNumber;
            orderHeader.CreationDate = orderHeaderDTO.CreationDate;
            orderHeader.AssignToEmployeeDate = orderHeaderDTO.AssignToEmployeeDate;
            orderHeader.FinalisationDate = orderHeaderDTO.FinalisationDate;

            _context.Entry(orderHeader).State = EntityState.Modified;

            var addressId = _context.Address
                            .FirstOrDefault(a => a.AddressName == orderHeaderDTO.Address)
                            .Id;

            var address = await _context.Address.FindAsync(addressId);

            if (address == null)
            {
                return NotFound();
            }

            address.AddressName = orderHeaderDTO.Address;

            _context.Entry(address).State = EntityState.Modified;

            var clientId = _context.Person
                            .FirstOrDefault(p => p.FirstName + ' ' + p.LastName == orderHeaderDTO.Client)
                            .Id;

            var client = await _context.Person.FindAsync(clientId);

            if (client == null)
            {
                return NotFound();
            }

            var clientSplit = orderHeaderDTO.Client.Split(' ');
            var clientFirstName = clientSplit[0];
            var clientLastName = clientSplit[1];
            
            client.FirstName = clientFirstName;
            client.LastName = clientLastName;

            _context.Entry(client).State = EntityState.Modified;

            var employeeId = _context.Person
                             .FirstOrDefault(p => p.FirstName + ' ' + p.LastName == orderHeaderDTO.Employee)
                             .Id;

            var employee = await _context.Person.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeSplit = orderHeaderDTO.Employee.Split(' ');
            var employeeFirstName = employeeSplit[0];
            var employeeLastName = employeeSplit[1];

            client.FirstName = employeeFirstName;
            client.LastName = employeeLastName;

            _context.Entry(employee).State = EntityState.Modified;

            var orderStateId = _context.OrderState
                            .FirstOrDefault(o => o.State == orderHeaderDTO.OrderState)
                            .Id;

            var orderState = await _context.OrderState.FindAsync(orderStateId);

            if (orderState == null)
            {
                return NotFound();
            }

            orderState.State = orderHeaderDTO.OrderState;

            _context.Entry(orderState).State = EntityState.Modified;

            var orderPriorityId = _context.OrderPriority
                            .FirstOrDefault(o => o.Priority == orderHeaderDTO.OrderPriority)
                            .Id;

            var orderPriority = await _context.OrderPriority.FindAsync(orderPriorityId);

            if (orderPriority == null)
            {
                return NotFound();
            }

            orderPriority.Priority = orderHeaderDTO.OrderPriority;

            _context.Entry(orderPriority).State = EntityState.Modified;

            try
            {
                await CreateOrEditOrderLines(orderHeader.OrderLines);
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

        private async Task CreateOrEditOrderLines(ICollection<OrderLine> orderLines)
        {
            ICollection<OrderLine> orderLinesToCreate = orderLines.Where(x => x.Id == 0).ToList();
            ICollection<OrderLine> orderLinesToEdit = orderLines.Where(x => x.Id != 0).ToList();

            if (orderLinesToCreate.Any())
            {
                await _context.AddRangeAsync(orderLinesToCreate);
            }

            if (orderLinesToEdit.Any())
            {
                _context.UpdateRange(orderLinesToEdit);
            }
        }

        // POST: api/OrderHeaders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OrderHeader>> PostOrderHeader(OrderHeader orderHeader)
        {
            _context.OrderHeader.Add(orderHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderHeader", new { id = orderHeader.Id }, orderHeader);
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

        private static OrderHeaderDTO OrderHeaderToDTO(OrderHeader orderHeader) {                      
            
            var orderheader = new OrderHeaderDTO
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
                OrderLines = orderHeader.OrderLines.Select(o => new OrderLineDTO
                    {
                        Id = o.Id,
                        OrderHeaderId = o.OrderHeaderId,
                        ProductName = o.Product.ProductName,
                        UnitPrice = o.UnitPrice,
                        Vat = o.Vat,
                        Quantity = o.Quantity
                    }).ToList()
            };

            return orderheader;
        }

       
    }
}
