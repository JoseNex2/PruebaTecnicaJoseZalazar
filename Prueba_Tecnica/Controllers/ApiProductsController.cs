using System.ComponentModel.DataAnnotations;
using System.Timers;
using DataAccess.Generic;
using entities.DataContext.Dtos;
using entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Prueba_Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApiProductController(IGenericRepository<Product> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _genericRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception m)
            {
                return StatusCode(500, "Error interno del sistema: " + m.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await _genericRepository.GetByIdAsync(id);
            if (products == null)
                return NotFound(new { msj = $"El producto con id {id} no existe" });
            else { return Ok(products); }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _unitOfWork.Context.categories.FindAsync(productDto.categoryId);
            if (category == null)
                return BadRequest(new { msj = "La categoría especificada no existe." });

            var product = new Product
            {
                name = productDto.name,
                description = productDto.description,
                price = productDto.price,
                quantity = productDto.quantity

            };
        
            try
            {
                if (await _genericRepository.CreateAsync(product))
                {
                    await _unitOfWork.CommitAsync();
                    return Ok(new { msj = "Producto creado con éxito." });
                }
                else
                {
                    return StatusCode(500, "Error al crear el producto.");
                }
            }
            catch (Exception m)
            {
                return StatusCode(500, "Error interno del sistema: " + m.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                id = id,
                name = productDto.name,
                description = productDto.description,
                price = productDto.price,
                quantity = productDto.quantity
            };

            try
            {
                if (await _genericRepository.UpdateAsync(id, product))
                {
                    await _unitOfWork.CommitAsync();
                    return Ok(new { mensaje = "Producto actualizado correctamente." });
                }

                return NotFound(new { msj = $"El producto con id {id} no existe" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del sistema: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _genericRepository.DeleteByIdAsync(id))
                {
                    await _unitOfWork.CommitAsync(); 
                    return Ok(new { message = "Producto borrado con exito" });
                }
                return NotFound(new {msj = $"El producto con id {id} no existe" });
            }
            catch (Exception m)
            {
                return StatusCode(500, "Error interno del sistema: " + m.Message);
            }
        }
    }
}
