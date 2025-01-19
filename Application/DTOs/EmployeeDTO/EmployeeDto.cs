using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDto
{
    public class EmployeeDto
    {
        public int EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int PositionId { get; set; }
        public decimal Salary { get; set; }
        public int? SupervisorCode { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Identification { get; set; }
        public string? PositionName { get; set; }
    }
}
