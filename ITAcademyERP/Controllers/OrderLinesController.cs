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
using ITAcademyERP.Data.DTOs;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Employee")]
    [ApiController]
    public class OrderLinesController : GenericController<Guid, OrderLine, OrderLinesRepository>
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
                ProductName = orderLine.Product.Name,
                UnitPrice = orderLine.UnitPrice,
                Vat = orderLine.Vat,
                Quantity = orderLine.Quantity
            };

            return orderLineDTO;
        }

        // GET: api/OrderLines
        [Route("TopProducts")]
        [HttpGet]
        public List<StatsByProduct> GetTopProducts()
        {
            var output = _repository.GetTopProducts();

            return output;
        }

        // GET: api/OrderLines
        [Route("SalesByProduct")]
        [HttpGet]
        public List<StatsByProduct> GetSalesByProduct()
        {
            var output = _repository.GetSalesByProduct();

            return output;
        }

        // GET: api/OrderLines
        [Route("TopClients")]
        [HttpGet]
        public List<StatsByClient> GetTopClients()
        {
            var output = _repository.GetTopClients();

            return output;
        }

        // GET: api/OrderLines
        [Route("SalesByClient")]
        [HttpGet]
        public List<StatsByClient> GetSalesByClient()
        {
            var output = _repository.GetSalesByClient();

            return output;
        }

        // GET: api/OrderLines
        [Route("SalesByDate")]
        [HttpGet]
        public List<StatsByDate> GetSalesByDate(string initialDate, string finalDate)
        {
            var output = _repository.GetSalesByDate(initialDate, finalDate);

            return output;
        }

        // GET: api/OrderLines
        [Route("SalesByDateAndProduct")]
        [HttpGet]
        public List<StatsByDate> GetSalesByDateAndProduct(string initialDate, string finalDate, string productName)
        {
            var output = _repository.GetSalesByDateAndProduct(initialDate, finalDate, productName);

            return output;
        }
    }
}
