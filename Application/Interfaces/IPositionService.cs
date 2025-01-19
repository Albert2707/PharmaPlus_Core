using Application.DTOs.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPositionService
    {
        Task CreatePosition(PositionDto position);
        Task<List<PositionDto>> GetAllPositionAsync();
        Task UpdatePosition(short id,PositionDto position);
        Task DeletePositionAsync(short id);
    }
}
