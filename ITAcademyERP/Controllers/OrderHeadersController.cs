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

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class OrderHeadersController : GenericController<OrderHeader, OrderHeadersRepository>
    {
        private readonly OrderHeadersRepository _repository;
        private readonly ClientsRepository _clientsRepository;
        private readonly EmployeesRepository _employeesRepository;
        private readonly OrderLinesController _orderLinesController;

        public OrderHeadersController(
            OrderHeadersRepository repository,
            ClientsRepository clientsRepository,
            EmployeesRepository employeesRepository,
            OrderLinesController orderLinesController) : base(repository)
        {
            _repository = repository;
            _clientsRepository = clientsRepository;
            _employeesRepository = employeesRepository;
            _orderLinesController = orderLinesController;
        }

        // GET: api/OrderHeaders
        [HttpGet]
        public async Task<IEnumerable<OrderHeaderDTO>> GetOrderHeaders()
        {
            var orderHeaders = await _repository.GetOrderHeaders();

            return orderHeaders.Select(e => OrderHeaderToDTO(e));
        }

        [Route("Employee")]
        [HttpGet]
        public async Task<IEnumerable<OrderHeaderDTO>> GetOrderHeadersByEmployee(int employeeId)
        {
            var orderHeaders = await _repository.GetOrderHeadersByEmployee(employeeId);

            return orderHeaders.Select(e => OrderHeaderToDTO(e));
        }

        [Route("Client")]
        [HttpGet]
        public async Task<IEnumerable<OrderHeaderDTO>> GetOrderHeadersByClient(int clientId)
        {
            var orderHeaders = await _repository.GetOrderHeadersByClient(clientId);

            return orderHeaders.Select(e => OrderHeaderToDTO(e));
        }

        //GET: api/OrderHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderHeaderDTO>> GetOrderHeader(int id)
        {
            var orderHeader = await _repository.GetOrderHeader(id);              

            if (orderHeader == null)
            {
                return NotFound();
            }

            return OrderHeaderToDTO(orderHeader);
        }

        // PUT: api/OrderHeaders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderHeader(OrderHeaderDTO orderHeaderDTO)
        {
            var orderHeader = await _repository.GetOrderHeader(orderHeaderDTO.Id);

            if (orderHeaderDTO.OrderState.ToString() == "Completada" && (OrderState)Enum.Parse(typeof(OrderState), orderHeaderDTO.OrderState) != orderHeader.OrderState)
            {
                orderHeader.FinalisationDate = DateTime.Now;
            }

            if (_employeesRepository.GetEmployeeId(orderHeaderDTO.Employee) != orderHeader.EmployeeId)
            {
                orderHeader.AssignToEmployeeDate = DateTime.Now;
            }

            orderHeader.OrderNumber = orderHeaderDTO.OrderNumber;
            orderHeader.ClientId = _clientsRepository.GetClientId(orderHeaderDTO.Client);
            orderHeader.EmployeeId = _employeesRepository.GetEmployeeId(orderHeaderDTO.Employee);
            orderHeader.OrderState = GetEnumValue<OrderState>(orderHeaderDTO.OrderState);
            orderHeader.OrderPriority = GetEnumValue<OrderPriority>(orderHeaderDTO.OrderPriority);

            return await _repository.Update(orderHeader);
        }       

        // POST: api/OrderHeaders
        [HttpPost]
        public async Task<ActionResult> PostOrderHeader(OrderHeaderDTO orderHeaderDTO)
        {            
            
            var orderHeader = new OrderHeader
            {
                OrderNumber = orderHeaderDTO.OrderNumber,
                ClientId = _clientsRepository.GetClientId(orderHeaderDTO.Client),
                EmployeeId = _employeesRepository.GetEmployeeId(orderHeaderDTO.Employee),
                OrderState = (OrderState)Enum.Parse(typeof(OrderState), orderHeaderDTO.OrderState),
                OrderPriority = (OrderPriority)Enum.Parse(typeof(OrderPriority), orderHeaderDTO.OrderPriority),
                CreationDate = DateTime.Now,
                AssignToEmployeeDate = DateTime.Now
            };

            return await _repository.Add(orderHeader);
        }        
        
        public OrderHeaderDTO OrderHeaderToDTO(OrderHeader orderHeader) {

            var orderLinesDTO = new List<OrderLineDTO>();

            foreach (var orderLine in orderHeader.OrderLines)
            {
                var orderLineDTO = _orderLinesController.OrderLineToDTO(orderLine);
                orderLinesDTO.Add(orderLineDTO);
            }           

            var orderHeaderDTO = new OrderHeaderDTO
            {
                Id = orderHeader.Id,
                OrderNumber = orderHeader.OrderNumber,
                Address = orderHeader.Client.Person.Addresses.FirstOrDefault(a => a.Type == "Delivery")?.Name,
                Client = orderHeader.Client.Person.FirstName + ' ' + orderHeader.Client.Person.LastName,
                Employee = orderHeader.Employee.Person.FirstName + ' ' + orderHeader.Employee.Person.LastName,
                OrderState = GetEnumString(orderHeader.OrderState),
                OrderPriority = GetEnumString(orderHeader.OrderPriority),
                CreationDate = orderHeader.CreationDate,
                AssignToEmployeeDate = orderHeader.AssignToEmployeeDate,
                FinalisationDate = orderHeader.FinalisationDate == null ? null : orderHeader.FinalisationDate,
                OrderLines = orderLinesDTO
            };

            return orderHeaderDTO;
        }
        
        public string GetEnumString<T>(T enumInput)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, enumInput);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            
            return enumMemberAttribute.Value;          
        }

        public static T GetEnumValue<T>(string enumName)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == enumName) return (T)Enum.Parse(enumType, name);
            }
            return default;
        }
    }
}
