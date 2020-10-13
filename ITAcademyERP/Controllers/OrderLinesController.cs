using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ITAcademyERP.Data;
using ITAcademyERP.Data.Repositories;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Employee")]
    [ApiController]
    public class OrderLinesController : GenericController<OrderLine, OrderLinesRepository>
    {
        private readonly OrderLinesRepository _repository;
        private readonly ProductsRepository _productsRepository;

        public OrderLinesController(
            OrderLinesRepository repository,
            ProductsRepository productsRepository) : base(repository)
        {
            _repository = repository;
            _productsRepository = productsRepository;
        }

        // GET: api/OrderLines
        [HttpGet]
        public async Task<IEnumerable<OrderLineDTO>> GetOrderLines()
        {
            var orderLines = await _repository.GetAll();

            return orderLines.Select(p => OrderLineToDTO(p));                
        }

        // POST: api/OrderLines
        [HttpPost]
        public async Task<ActionResult> AddOrderLine(OrderLineDTO orderLineDTO)
        {
            var orderLine = new OrderLine
            {
                Id = orderLineDTO.Id,
                OrderHeaderId = orderLineDTO.OrderHeaderId,
                ProductId = _productsRepository.GetProductId(orderLineDTO.ProductName),
                UnitPrice = orderLineDTO.UnitPrice,
                Vat = orderLineDTO.Vat,
                Quantity = orderLineDTO.Quantity
            };

            return await _repository.Add(orderLine);
        }

        public OrderLineDTO OrderLineToDTO(OrderLine orderLine)
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
