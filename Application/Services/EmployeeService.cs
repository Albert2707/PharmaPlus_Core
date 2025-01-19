using Application.Common;
using Application.DTOs.EmployeeDto;
using Application.Interfaces;
using CloudinaryDotNet.Core;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            try
            {
                Logs.Info("Buscando empleados...");
                var emps = await _context.Employees.Include(e => e.Position).ToListAsync();
                return emps.Select(e => new EmployeeDto
                {
                    EmployeeCode = e.EmployeeCode,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Identification=e.Identification,
                    Address = e.Address,
                    Salary = e.Salary,
                    SupervisorCode = e.SupervisorCode,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    PositionName = e.Position.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            try
            {
                Logs.Info("Buscando empleados");
                var emp = await _context.Employees
                    .Include(e => e.Position) // Asegura que la relación esté cargada
                    .FirstOrDefaultAsync(e => e.EmployeeCode == id);

                if (emp == null)
                {
                    var errorMessage = $"No existe el empleado con identificador {id}";
                    throw new KeyNotFoundException(errorMessage); // Usa una excepción más específica
                }

                return new EmployeeDto
                {
                    FirstName = emp.FirstName ?? string.Empty, // Manejo de valores nulos
                    LastName = emp.LastName ?? string.Empty,
                    Address = emp.Address ?? string.Empty,
                    Salary = emp.Salary,
                    SupervisorCode = emp.SupervisorCode ?? null,
                    Phone = emp.Phone ?? string.Empty,
                    DateOfBirth = emp.DateOfBirth,
                    PositionName = emp.Position?.Name ?? "Sin posición asignada" // Manejo de relaciones nulas
                };
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return new EmployeeDto();
            }
        }

        public async Task<List<EmployeeDto>> GetEmployeeByName(string name)
        {
            try
            {
                Logs.Info("Buscando empleado por nombre");
                var emp = await _context.Employees.Include(e=>e.Position).Where(e => e.FirstName == name).ToListAsync();
                return emp.Select(e => new EmployeeDto
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Address = e.Address,
                    Salary = e.Salary,
                    SupervisorCode = e.SupervisorCode,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    PositionName = e.Position.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }

        public async Task<List<EmployeeDto>> GetEmployeesByPosition(int positionId)
        {
            try
            {
                Logs.Info("Buscando empleado por posicion");
                var emp = await _context.Employees.Include(e => e.Position).Where(e => e.PositionId == positionId).ToListAsync();
                return emp.Select(e => new EmployeeDto
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Address = e.Address,
                    Salary = e.Salary,
                    SupervisorCode = e.SupervisorCode,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    PositionName = e.Position.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }

        public async Task CreateEmployee(EmployeeDto emp)
        {
            try
            {
                Logs.Info("Creando empleado...");
                if (emp == null)
                {
                    throw new Exception("Debe proporcionar los datos del empleado");
                }
                var NewEmp = new Employee()
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Address = emp.Address,
                    PositionId =emp.PositionId,
                    Salary = emp.Salary,
                    SupervisorCode = emp.SupervisorCode,
                    Phone = emp.Phone,
                    DateOfBirth = emp.DateOfBirth,
                    Identification = emp.Identification
                };
                _context.Employees.Add(NewEmp);
                await _context.SaveChangesAsync();
                Logs.Info("Empleado agregado exitosamente");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }

        public async Task UpdateEmployee(int id, EmployeeDto emp)
        {
            try
            {
                var e = await _context.Employees.FindAsync(id)??throw new Exception($"Empleado con el identificador {id} no existe");
                e.FirstName = emp.FirstName ?? e.FirstName;
                e.LastName = emp.LastName ?? e.LastName;
                e.Address = emp.Address ?? e.Address;
                e.PositionId = emp.PositionId == 0 ? e.PositionId : emp.PositionId;
                e.Salary = emp.Salary == 0 ? e.Salary : emp.Salary;
                e.SupervisorCode = emp.SupervisorCode ?? e.SupervisorCode;
                e.Phone = emp.Phone ?? e.Phone;
                e.DateOfBirth = emp.DateOfBirth == DateTime.MinValue ? e.DateOfBirth : emp.DateOfBirth;
                e.Identification = emp.Identification ?? e.Identification;
                _context.Employees.Update(e);
                await _context.SaveChangesAsync();
                Logs.Info("Empleado actualizado exitosamente");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }

        public async Task DeleteEmployee(int id)
        {
            try
            {
                Logs.Info("Eliminando empleado... :(");
                var emp = await _context.Employees.FindAsync(id)??throw new Exception($"El empleado con el identificador {id} no existe");
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
                Logs.Info("Empleado Eliminado exitosamente");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
    }
}