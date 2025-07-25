using System.ComponentModel.DataAnnotations;
using System.Timers;
using Contracts;
using DataAccess.Generic;
using entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Prueba_Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiProductController : ControllerBase
    {
        //private readonly IGenericRepository<Product> _genericRepository;
        //private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public ApiProductController(IProductService productService)
        { 
            //_genericRepository = genericRepository;
            //_unitOfWork = unitOfWork;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllProductsAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(400, result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (result.Success && result.Data != null)
                return Ok(result);

            return NotFound(new { msj = result.Message });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDto)
        {

            var result = await _productService.CreateProductAsync(productDto);

            if (result.Success && result.Data != null)
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            
            return StatusCode(500, new {msj = result.Message});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDto)
        {

            var result = await _productService.UpdateProductAsync(id, productDto); 

            if (result.Success)
                return Ok(result.Data);

            return NotFound(new { msj = result.Message });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _productService.DeleteAsync(id);

            if (result.Success && result.Data)
                return Ok(new { result.Message });

            if (!result.Success)
                return StatusCode(500, new { msj = result.Message });

            return NotFound(new { result.Message });

        }
    }
}
