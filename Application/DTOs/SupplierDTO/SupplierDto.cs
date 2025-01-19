using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.SupplierDTO
{
    public class SupplierDto
    {
        public string? RNC { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public List<string>? ProductNames { get; set; }
    }
}
