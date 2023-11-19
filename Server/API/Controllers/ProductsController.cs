using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specification;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase
    {
        //private readonly IProductRepository _repository;
        private readonly IGenericRepository<Product> _productRepo;
        private IGenericRepository<ProductBrand> _productBandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
        IGenericRepository<ProductBrand> productBandRepo,
         IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _productRepo=productRepo;
            _productBandRepo=productBandRepo;
            _productTypeRepo=productTypeRepo;
            _mapper=mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var spec= new ProductWithTypesAndBrandsSpecification();
            var products=await _productRepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec= new ProductWithTypesAndBrandsSpecification(id);
            var product=await _productRepo.GetEntityWithSpec(spec);
             var data = _mapper.Map<ProductToReturnDto>(product);
            return Ok(data);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            return Ok(await _productBandRepo.GetAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            return Ok(await _productTypeRepo.GetAllAsync());
        }
    }
}