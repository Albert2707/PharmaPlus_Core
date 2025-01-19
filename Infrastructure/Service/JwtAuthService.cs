using Domain.Entities;
using Domain.Interface;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class JwtAuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        //public JwtService()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    _config = builder.Build();
        //}
        public JwtAuthService(IConfiguration config, AppDbContext context)
        {
            _context = context;
            _config = config;
        }
        public async Task<string> GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
            new(ClaimTypes.PrimarySid, user.UserId.ToString()),
            new(ClaimTypes.Email, user.Email),
        };
            var roles = await _context.UserRoles
       .Where(ur => ur.UserId == user.UserId)
       .Include(ur => ur.Role)
       .Select(ur => ur.Role.Name)
       .ToListAsync();

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> ValidateToken(string token)
        {
            return true;
        }
    }
}
