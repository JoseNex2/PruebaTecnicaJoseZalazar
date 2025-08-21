using Contracts;
using DataAccess.Services2;
using entities.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Timers;

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
                return Ok(result);
            }
            return StatusCode(400, result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (result.Success && result.Response != null)
            {
                return StatusCode((int)HttpStatusCode.OK, result);
            }

            return StatusCode((int)HttpStatusCode.NotFound, result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDto)
        {

            var result = await _productService.CreateProductAsync(productDto);

            if (result.Success && result.Response != null)
                return CreatedAtAction(nameof(GetById), new { id = result.Response.Id }, result.Response);
            
            return StatusCode(500, new {msj = result.Message});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDto)
        {

            var result = await _productService.UpdateProductAsync(id, productDto); 

            if (result.Success)
                return Ok(result.Response);

            return NotFound(new { msj = result.Message });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _productService.DeleteAsync(id);

            if (result.Success && result.Response)
                return StatusCode((int)HttpStatusCode.OK, result);

            if (!result.Success)
                return StatusCode((int)HttpStatusCode., result);

            return NotFound(new { result.Message });

        }
    }
}
