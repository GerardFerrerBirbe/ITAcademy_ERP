using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using ITAcademyERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ITAcademyERP.Data.Repositories;
using System.ComponentModel;
using System.Runtime.Serialization;
using ITAcademyERP.Data.DTOs;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class OrderHeadersController : GenericController<Guid, OrderHeader, OrderHeadersRepository>
    {
        private readonly OrderHeadersRepository _repository;

        public OrderHeadersController(
            OrderHeadersRepository repository) : base(repository)
        {
            _repository = repository;
        }

        // GET: api/OrderHeaders
        [HttpGet]
        public async Task<IEnumerable<OrderHeader>> GetOrderHeaders()
        {
            var orderHeaders = await _repository.GetAll();

            return orderHeaders.Select(o => FilteringDeliveryAddress(o));
        }

        [Route("Employee")]
        [HttpGet]
        public async Task<IEnumerable<OrderHeader>> GetOrderHeadersByEmployee(Guid employeeId)
        {
            var orderHeaders = await _repository.GetOrderHeadersByEmployee(employeeId);

            return orderHeaders;
        }

        [Route("Client")]
        [HttpGet]
        public async Task<IEnumerable<OrderHeader>> GetOrderHeadersByClient(Guid clientId)
        {
            var orderHeaders = await _repository.GetOrderHeadersByClient(clientId);

            return orderHeaders;
        }

        //GET: api/OrderHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderHeader>> GetOrderHeader(Guid id)
        {
            var orderHeader = await _repository.Get(id);              

            if (orderHeader == null)
            {
                return NotFound();
            }

            return orderHeader;
        }

        // PUT: api/OrderHeaders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderHeader(OrderHeader orderHeaderUpdate)
        {
            var orderHeader = await _repository.Get(orderHeaderUpdate.Id);

            if (orderHeaderUpdate.OrderState == OrderState.Completada && orderHeaderUpdate.OrderState != orderHeader.OrderState)
            {
                orderHeader.FinalisationDate = DateTime.Now;
            }

            if (orderHeaderUpdate.Employee.Id != orderHeader.EmployeeId)
            {
                orderHeader.AssignToEmployeeDate = DateTime.Now;
            }

            orderHeader.OrderNumber = orderHeaderUpdate.OrderNumber;
            orderHeader.ClientId = orderHeaderUpdate.Client.Id;
            orderHeader.EmployeeId = orderHeaderUpdate.Employee.Id;
            orderHeader.OrderState = orderHeaderUpdate.OrderState;
            orderHeader.OrderPriority = orderHeaderUpdate.OrderPriority;

            return await _repository.Update(orderHeader);
        }       

        // POST: api/OrderHeaders
        [HttpPost]
        public async Task<ActionResult> PostOrderHeader(OrderHeader newOrderHeader)
        {
            var orderHeader = new OrderHeader
            {
                OrderNumber = newOrderHeader.OrderNumber,
                ClientId = newOrderHeader.Client.Id,
                EmployeeId = newOrderHeader.Employee.Id,
                OrderState = newOrderHeader.OrderState,
                OrderPriority = newOrderHeader.OrderPriority,
                CreationDate = DateTime.Now,
                AssignToEmployeeDate = DateTime.Now
            };

            return await _repository.Add(orderHeader);
        }

        public OrderHeader FilteringDeliveryAddress(OrderHeader orderHeader)
        {
            return new OrderHeader
            {
                Id = orderHeader.Id,
                OrderNumber = orderHeader.OrderNumber,
                Client = new Client
                {
                    Id = orderHeader.Client.Id,
                    Person = new Person
                    {
                        Id = orderHeader.Client.Person.Id,
                        FirstName = orderHeader.Client.Person.FirstName,
                        LastName = orderHeader.Client.Person.LastName,
                        Email = orderHeader.Client.Person.Email,
                        Addresses = orderHeader.Client.Person.Addresses.Where(a => a.Type == AddressType.Entrega).ToList()
                    }
                },
                Employee = orderHeader.Employee,
                OrderState = orderHeader.OrderState,
                OrderPriority = orderHeader.OrderPriority,
                CreationDate = orderHeader.CreationDate,
                AssignToEmployeeDate = orderHeader.AssignToEmployeeDate,
                FinalisationDate = orderHeader.FinalisationDate,
                OrderLines = orderHeader.OrderLines
            };
        }
    }
}
