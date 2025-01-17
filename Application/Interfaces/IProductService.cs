using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(CreateProductDto productDto);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task UpdateProduct(int id, UpdateProductDto product);
        Task DeleteProduct(int id);
        Task<List<ProductDto>> GetProductByCategory(int CategoryId);
    }
}
