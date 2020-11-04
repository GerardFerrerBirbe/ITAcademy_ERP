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
using ITAcademyERP.Data.DTOs;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class ProductsController : GenericController<Guid, Product, ProductsRepository>
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
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _repository.GetAll();

            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _repository.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Product productUpdate)
        {
            var product = await _repository.Get(productUpdate.Id);

            product.Name = productUpdate.Name;
            product.CategoryId = _productCategoriesRepository.GetProductCategoryId(productUpdate.Category.Name);

            return await _repository.Update(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product newProduct)
        {
            var product = new Product
            {
                Name = newProduct.Name,
                CategoryId = _productCategoriesRepository.GetProductCategoryId(newProduct.Category.Name)
            };

            return await _repository.Add(product);
        }      
    }
}
