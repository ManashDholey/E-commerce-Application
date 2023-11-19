using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase
    {
        private readonly IProductRepository _repository;
        public ProductsController(IProductRepository repository )
        {
            _repository=repository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products=await _repository.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product=await _repository.GetProductByIdAsync(id);
            return Ok(product);
        }
    }
}