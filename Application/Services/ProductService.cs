using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Azure.Core;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        readonly CloudinaryConfig cloudDinary;

        public ProductService(AppDbContext context, CloudinaryConfig cd)
        {
            _context = context;
            cloudDinary  =cd;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                Logs.Info("Peticion para mostrar todos los productos");
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Type)
                    .ToListAsync();

                var product = products.Select(p => new ProductDto
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
                Logs.Info("Retornando productos...");
                return product;
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }

        public async Task<List<ProductDto>> GetProductByCategory(int CategoryId)
        {
            try
            {
                Logs.Info("Buscando productos por categoria...");
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
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }
        public async Task AddProductAsync(CreateProductDto productDto)
        {
            try
            {
                Logs.Info("Agregando nuevo producto...");
                if (productDto.file == null || productDto.file.Length == 0)
                {
                    throw new Exception("No se ha proporcionado ningún archivo");
                }
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
                Logs.Info("Agregando nuevo producto...");
                await _context.SaveChangesAsync();
                Logs.Info("Producto Agregado");

            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }

        public async Task UpdateProduct(int id, UpdateProductDto productDto)
        {
            try
            {
                Logs.Info("Actualizando nuevo producto...");
                var product = await _context.Products.FindAsync(id)??throw new Exception("Product not found");
                string SecureUrl = product.ImgUrl;
                string PublicImgId = product.publicImgId;

                if (productDto.file != null && productDto.file.Length > 0)
                {
                    string uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(productDto.file.FileName)}";
                    using var stream = productDto.file.OpenReadStream();
                    var resultCloudinary = cloudDinary.Upload(uniqueName, stream);
                    SecureUrl = resultCloudinary.SecureUrl;
                    PublicImgId = resultCloudinary.PublicId;
                    cloudDinary.Destroy(product.publicImgId);
                }

                product.Name = productDto.Name ?? product.Name;
                product.Description = productDto.Description ?? product.Description;
                product.Price = productDto.Price == 0 ? product.Price : productDto.Price;
                product.ImgUrl = SecureUrl;
                product.Stock = productDto.Stock == 0 ? product.Stock : productDto.Stock;
                product.publicImgId = PublicImgId;
                product.CategoryId = productDto.CategoryId;
                product.TypeId = productDto.ProductTypeId;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                Logs.Info("Producto actualizado");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id)??throw new Exception("Product not found");
                cloudDinary.Destroy(product.publicImgId);
                _context.Products.Remove(product);
                Logs.Info("Eliminando nuevo producto...");
                await _context.SaveChangesAsync();
                Logs.Info("Producto eliminado");
            }
            catch (Exception ex)
            {
                Logs.Error("No se pudo eliminar el producto", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
