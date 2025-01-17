using Application.DTOs;
using Application.Interfaces;
using Azure.Core;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        CloudinaryConfig cloudDinary = new();

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Type)
                    .ToListAsync();

                var esto = products.Select(p => new ProductDto
                {
                    Id = p.ProductCode,
                    Name = p.Name ?? "No Name",
                    Description = p.Description ?? "No Description",
                    ImgUrl = p.ImgUrl,
                    CategoryName = p.Category?.Name ?? "No Category",
                    ProductTypeName = p.Type?.Name ?? "No Type",
                    Price = p.Price, // Si Price es nullable
                    Stock = p.Stock  // Si Stock es nullable
                }).ToList();

                return esto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllProductsAsync: {ex.Message}");
                throw;
            }
        }
        
        public async Task<List<ProductDto>> GetProductByCategory(int CategoryId)
        {
            try
            {
                var product = await _context.FilterProductsByCategoryAsync(CategoryId);
                var products = new List<ProductDto>();
                foreach (var item in product)
                {
                    var proDto = new ProductDto()
                    {
                        Id = item.ProductCode,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        ImgUrl = item.ImgUrl,
                        Stock = item.Stock,
                        CategoryName = item.CategoryName
                    };
                    products.Add(proDto);

                }
                return products;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task AddProductAsync(CreateProductDto productDto)
        {
            try
            {
                if (productDto.file == null || productDto.file.Length == 0) throw new Exception("No se ha proporcionado ningún archivo");
                string uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(productDto.file.FileName)}";
                using var stream = productDto.file.OpenReadStream();
                var resultCloudinary = cloudDinary.Upload(uniqueName, stream);
                var product = new Product()
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    ImgUrl = resultCloudinary.SecureUrl,
                    Stock = productDto.Stock,
                    publicImgId = resultCloudinary.PublicId,
                    CategoryId = productDto.CategoryId,
                    TypeId = productDto.ProductTypeId

                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateProduct(int id, UpdateProductDto productDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product  == null)
                {
                    throw new Exception("Product not found");
                }
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.ImgUrl = productDto.ImgUrl;
                product.Stock = productDto.Stock;
                product.CategoryId = productDto.CategoryId;
                product.TypeId = productDto.ProductTypeId;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product  == null)
                {
                    throw new Exception("Product not found");
                }
                cloudDinary.Destroy(product.publicImgId);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
