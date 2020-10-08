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

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class ProductsController : GenericController<Product, ProductRepository>
    {
        private readonly ProductRepository _repository;
        public ProductsController(ProductRepository repository) : base(repository)
        {
            _repository = repository;
        }

        //GET: api/Products
       [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetProduct()
        {
            var product = await _repository.GetProducts();

            return product.Select(p => ProductToDTO(p));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return ProductToDTO(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(ProductDTO productDTO)
        {
            return await _repository.UpdateProduct(productDTO);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            return await _repository.AddProduct(productDTO);
        }

        public static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductCategoryName = product.ProductCategory.ProductCategoryName
            };

        public int GetProductId(string productName)
        {
            var productId = /*_repository.Products.FirstOrDefault(x => x.ProductName == productName).Id;*/ 9;

            return productId;
        }        
    }
}
