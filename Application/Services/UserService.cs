using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUser(string email, string password)
        {
            try
            {

            Logs.Info("Autenticando usuario...");
            string s = BCrypt.Net.BCrypt.EnhancedHashPassword("hola", 12);
            Logs.Info(s);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email) ?? throw new Exception("Correo o Contraseña incorrectos");
            bool match = BCrypt.Net.BCrypt.EnhancedVerify(password,user.Password);
            if (!match) throw new Exception("Correo o Contraseña incorrectos");
            return user;
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return new User();
            }
        }
    }
}
