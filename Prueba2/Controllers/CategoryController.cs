using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Prueba2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _cat;
        public CategoryController(ICategoryService cat)
        {
            _cat=cat;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            try
            {
                await _cat.AddCategory(category);
                return Ok("Categoría creada exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
