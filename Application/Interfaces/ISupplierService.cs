using Application.DTOs.SupplierDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierDto>> GetAllSuppliers();
        Task CreateSupplier(SupplierDto supplier);
        Task UpdateSupplier(string rnc, SupplierDto supplier);
        Task DeleteSupplier(string rnc);
    }
}
