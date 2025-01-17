using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public DateTime DateTime { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }
        public int ProductCode { get; set; }
        public int EmployeeCode { get; set; }
        public int ClientId { get; set; }

        // Relationships
        public Product Product { get; set; }
        public Employee Employee { get; set; }
        public Customer Customer{ get; set; }
        public Invoice Invoice { get; set; }

    }
}
