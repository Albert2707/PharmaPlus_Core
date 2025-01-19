using Application.DTOs.SupplierDTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _sup;
        public SupplierController(ISupplierService sup)
        {
            _sup=sup;
        }

        [Authorize(Roles = "admin, employee")]
        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            try
            {
                var res = await _sup.GetAllSuppliers();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSuppliers(SupplierDto supplier)
        {
            try
            {
                await _sup.CreateSupplier(supplier);
                return Ok(new { message = "Suplidor creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{rnc}")]
        public async Task<IActionResult> UpdateSuppliers(string rnc, SupplierDto supplier)
        {
            try
            {
                await _sup.UpdateSupplier(rnc, supplier);
                return Ok(new { message = "Suplidor actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{rnc}")]
        public async Task<IActionResult> DeleteSuppliers(string rnc)
        {
            try
            {
                await _sup.DeleteSupplier(rnc);
                return Ok(new { message = "Suplidor eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
