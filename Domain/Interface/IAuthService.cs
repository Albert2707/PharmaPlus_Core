using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
        Task<bool> ValidateToken(string token);
    }
}
