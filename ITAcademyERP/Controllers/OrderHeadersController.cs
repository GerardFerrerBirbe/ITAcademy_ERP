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
using ITAcademyERP.Data.Resources;

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
        public override async Task<IEnumerable<OrderHeader>> GetAll()
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

        // PUT: api/OrderHeaders/5
        [HttpPut("{id}")]
        public override async Task<IActionResult> Put(OrderHeader inputOrderHeader)
        {
            var orderHeader = await _repository.Get(inputOrderHeader.Id);

            if (inputOrderHeader.OrderState == EOrderState.Completada && inputOrderHeader.OrderState != orderHeader.OrderState)
            {
                orderHeader.FinalisationDate = DateTime.Now;
            }

            if (inputOrderHeader.EmployeeId != orderHeader.EmployeeId)
            {
                orderHeader.AssignToEmployeeDate = DateTime.Now;
            }

            orderHeader.OrderNumber = inputOrderHeader.OrderNumber;
            orderHeader.ClientId = inputOrderHeader.ClientId;
            orderHeader.EmployeeId = inputOrderHeader.EmployeeId;
            orderHeader.OrderState = inputOrderHeader.OrderState;
            orderHeader.OrderPriority = inputOrderHeader.OrderPriority;

            return await _repository.Update(orderHeader);
        }       

        // POST: api/OrderHeaders
        [HttpPost]
        public override async Task<ActionResult> Post(OrderHeader inputOrderHeader)
        {
            var orderHeader = new OrderHeader
            {
                OrderNumber = inputOrderHeader.OrderNumber,
                ClientId = inputOrderHeader.ClientId,
                EmployeeId = inputOrderHeader.EmployeeId,
                OrderState = inputOrderHeader.OrderState,
                OrderPriority = inputOrderHeader.OrderPriority,
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
                        Addresses = orderHeader.Client.Person.Addresses.Where(a => a.Type == EAddressType.Entrega).ToList()
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
