using DataAccess.Generic;
using entities.DataContext.Dtos;
using entities.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Prueba_Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiCategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApiCategoryController(IGenericRepository<Category> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _genericRepository.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception m)
            {
                return StatusCode(500, "Internal server error: " + m.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categories = await _genericRepository.GetByIdAsync(id);
            if (categories == null)
                return NotFound(new { msj = $"La categoria con id {id} no existe" });
            else
            {
                return Ok(categories);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                name = categoryDto.name
            };

            try
            {
                var created = await _genericRepository.CreateAsync(category);
                if (created)
                {
                    await _unitOfWork.CommitAsync();
                    return Ok(new { msj = "Categoría creada con éxito" });
                }
                return StatusCode(500, "Error al crear la categoría.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _genericRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { msj = $"La categoría con id {id} no existe" });

            var verifyProducts = await _unitOfWork.Context.products.AnyAsync(p => p.categoryId == id);
            if (verifyProducts)
                return BadRequest(new { msj = "No se puede eliminar la categoría porque tiene productos asociados" });
            
            if (await _genericRepository.DeleteByIdAsync(id))
            {
                await _unitOfWork.CommitAsync();
                return Ok(new { msj = "Categoría eliminada con éxito." });
            }

            return StatusCode(500, "Error al eliminar la categoría.");
        }
    }
}
