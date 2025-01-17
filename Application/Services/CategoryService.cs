using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                var newCategory = new Domain.Entities.Category()
                {
                    Name = category.Name
                };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories.Select(e => new CategoryDto
                {
                    CategoryId = e.CategoryId,
                    Name = e.Name
                }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) throw new Exception("Categoria no encontrada");
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
