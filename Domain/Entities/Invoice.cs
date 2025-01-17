using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invoice
    {
        public int InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public int SaleId { get; set; }
        public int ClientId { get; set; }
        public ICollection<Sale> Sales { get; set; }

    }
}
