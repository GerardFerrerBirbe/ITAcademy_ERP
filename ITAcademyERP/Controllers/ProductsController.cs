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
    public class ProductsController : GenericController<Product, ProductsRepository>
    {
        private readonly ProductsRepository _repository;
        private readonly ProductCategoriesRepository _productCategoriesRepository;
        public ProductsController(
            ProductsRepository repository,
            ProductCategoriesRepository productCategoriesRepository) : base(repository)
        {
            _repository = repository;
            _productCategoriesRepository = productCategoriesRepository;
        }

        //GET: api/Products
       [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = await _repository.GetProducts();

            return products.Select(p => ProductToDTO(p));
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
            var product = await _repository.GetProduct(productDTO.Id);

            product.ProductName = productDTO.ProductName;
            product.ProductCategoryId = _productCategoriesRepository.GetProductCategoryId(productDTO.ProductCategoryName);

            return await _repository.Update(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            var product = new Product
            {
                ProductName = productDTO.ProductName,
                ProductCategoryId = _productCategoriesRepository.GetProductCategoryId(productDTO.ProductCategoryName)
            };

            return await _repository.Add(product);
        }

        public static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductCategoryName = product.ProductCategory.ProductCategoryName
            };      
    }
}
