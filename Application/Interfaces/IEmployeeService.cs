using Application.DTOs.EmployeeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllEmployees();
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<List<EmployeeDto>> GetEmployeeByName(string name);
        Task<List<EmployeeDto>> GetEmployeesByPosition(int positionId);
        Task CreateEmployee(EmployeeDto emp);
        Task UpdateEmployee(int id, EmployeeDto emp);
        Task DeleteEmployee(int id);
    }
}
