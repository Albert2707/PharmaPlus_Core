using Application.DTOs.Position;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly IPositionService _po;
        public PositionController(IPositionService po)
        {
            _po=po;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositions()
        {
            try
            {
                var res = await _po.GetAllPositionAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosition(PositionDto position)
        {
            try
            {
                await _po.CreatePosition(position);
                return Ok(new { message = "Posicion creada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(short id)
        {
            try
            {
                await _po.DeletePositionAsync(id);
                return Ok(new { message="Posicion eliminada exitosamente"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
