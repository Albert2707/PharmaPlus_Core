using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Position
    {
        public int PositionId { get; set; }
        public string Name { get; set; }

        // Relationship with Employee
        public ICollection<Employee> Employees { get; set; }
    }
}
