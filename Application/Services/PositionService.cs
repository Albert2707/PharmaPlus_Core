using Application.Common;
using Application.DTOs.Position;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PositionService : IPositionService
    {
        private readonly AppDbContext _context;
        public PositionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PositionDto>> GetAllPositionAsync()
        {
            try
            {
                Logs.Info("Buscando posiciones...");
                var positions = await _context.Positions.ToListAsync();
                var pos = positions.Select(e => new PositionDto
                {
                    PositionId = e.PositionId,
                    Name = e.Name,
                }).ToList();
                return pos;
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
                return [];
            }
        }

        public async Task CreatePosition(PositionDto position)
        {
            try
            {
                Logs.Info("Agregando nueva posicion...");
                if (position == null)
                {
                    throw new Exception("Debe enviar datos para crear la nueva posicion");
                }
                var po = new Position()
                {
                    Name = position.Name,
                };
                _context.Positions.Add(po);
                await _context.SaveChangesAsync();
                Logs.Info("Posicion agregada exitosamente.");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }

        }

        public async Task UpdatePosition(short id, PositionDto position)
        {
            try
            {
                Logs.Info($"Actualizando posicion con identificador {id}");
                var pos = _context.Positions.Find(id)??throw new Exception($"La posicion con identificador {id} no existe");
                pos.Name = position.Name ?? pos.Name;
                _context.Positions.Update(pos);
                await _context.SaveChangesAsync();
                Logs.Info("Posicion actualizada");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
        public async Task DeletePositionAsync(short id)
        {
            try
            {
                Logs.Info("Eliminando posicion...");
                var position = await _context.Positions.FindAsync(id)??throw new Exception($"La posicion con el identificador {id} no existe");
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
                Logs.Info("Posicion eliminada");
            }
            catch (Exception ex)
            {
                ExceptionMessage.CatchException(ex);
            }
        }
    }
}
