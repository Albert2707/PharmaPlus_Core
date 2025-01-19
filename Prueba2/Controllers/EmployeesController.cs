using Application.DTOs.EmployeeDto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : Controller
{
    private readonly IEmployeeService _emp;
    public EmployeesController(IEmployeeService emp) => _emp = emp;

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var employees = await _emp.GetAllEmployees();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        try
        {
            var employee = await _emp.GetEmployeeById(id);
            if (employee == null)
                return NotFound($"Empleado {id} no encontrado");
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetEmployeesByName(string name)
    {
        try
        {
            var employees = await _emp.GetEmployeeByName(name);
            if (!employees.Any())
                return NotFound($"No se encontraron empleados con nombre {name}");
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpGet("position/{positionId}")]
    public async Task<IActionResult> GetEmployeesByPosition(int positionId)
    {
        try
        {
            var employees = await _emp.GetEmployeesByPosition(positionId);
            if (!employees.Any())
                return NotFound($"No se encontraron empleados en la posición {positionId}");
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeDto emp)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _emp.CreateEmployee(emp);
            return Ok(new { message = "Empleado creado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto emp)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _emp.UpdateEmployee(id, emp);
            return Ok(new { message = "Empleado actualizado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            await _emp.DeleteEmployee(id);
            return Ok(new { message = "Empleado eliminado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }
}