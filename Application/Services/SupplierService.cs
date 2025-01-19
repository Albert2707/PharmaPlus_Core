using Application.Common;
using Application.DTOs.SupplierDTO;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SupplierService: ISupplierService
    {
        private readonly AppDbContext _context;
        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierDto>> GetAllSuppliers()
        {
            try
            {
                Logs.Info("Buscando Suplidores...");
                var supplier = await _context.Suppliers.Include(e => e.ProductCatalogs).ThenInclude(c => c.Product).ToListAsync();
                return supplier.Select(e => new SupplierDto
                {
                    RNC = e.RNC,
                    Name = e.Name,
                    Phone = e.Phone,
                    Email = e.Email,
                    ProductNames = e.ProductCatalogs.Select(e => e.Product.Name).ToList(),

                }).ToList();

            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }
        public async Task CreateSupplier(SupplierDto supplier)
        {
            try
            {
                List<string> exlcuded = [ "ProductNames" ];
                ValidateNonNullableProperties(supplier, exlcuded);
                Logs.Info("Creando suplidor");
                var NewSupplier = new Supplier()
                {
                    RNC = supplier.RNC!,
                    Name =supplier.Name!,
                    Phone = supplier.Phone!,
                    Email = supplier.Email!

                };
                _context.Suppliers.Add(NewSupplier);
                await _context.SaveChangesAsync();
                Logs.Info("Suplidor creado exitosamente");

            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
        public async Task UpdateSupplier(string rnc, SupplierDto supplier)
        {
            try
            {
                Logs.Info("Actualizando suplidor...");
                List<string> exlcuded = ["ProductNames","RNC"];
                ValidateNonNullableProperties(supplier, exlcuded);
                var sup = await _context.Suppliers.FirstOrDefaultAsync(e => e.RNC == rnc)??throw new Exception($"Suplidor con el RCN:{rnc} no existe");
                sup.Name = supplier.Name ?? sup.Name;
                sup.Phone = supplier.Phone ?? sup.Phone;
                sup.Email = supplier.Email ?? sup.Email;
                _context.Suppliers.Update(sup);
                await _context.SaveChangesAsync();
                Logs.Info("Suplidor actualizado exitosamente");

            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
        public async Task DeleteSupplier(string rnc)
        {
            try
            {
                Logs.Info("Eliminando suplidor... :(");
                var sup = await _context.Suppliers.FirstOrDefaultAsync(e => e.RNC == rnc)??throw new Exception($"Suplidor con el RCN:{rnc} no existe");
                _context.Suppliers.Remove(sup);
                await _context.SaveChangesAsync();
                Logs.Info("Suplidor eliminado exitosamente");

            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
        public static void ValidateNonNullableProperties<T>(T obj, List<string> excludedProperties)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(obj) == null && !excludedProperties.Contains(property.Name))
                {
                    throw new Exception($"La propiedad {property.Name} no puede ser nula.");
                }
            }
        }
    }
}
