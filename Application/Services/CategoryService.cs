using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCategory(CategoryDto category)
        {
            try
            {
                var newCategory = new Category()
                {
                    Name = category.Name
                };
                _context.Categories.Add(newCategory);

                Logs.Info("Agregando nueva categoria...");
                await _context.SaveChangesAsync();
                Logs.Info("Categoria agregada");

            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                Logs.Info("Buscando categorias...");
                return await _context.Categories.Select(e => new CategoryDto
                {
                    CategoryId = e.CategoryId,
                    Name = e.Name
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                string ErrorMessage = "Categoria no encontrada";
                if (category == null)
                {
                    throw new Exception(ErrorMessage);
                }
                _context.Categories.Remove(category);
                Logs.Info("Eliminando categoria...");
                await _context.SaveChangesAsync();
                Logs.Info("Categoria eliminada");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
    }
}
