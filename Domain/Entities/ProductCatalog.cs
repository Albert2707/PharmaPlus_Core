using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductCatalog
    {
        public int ProductCode { get; set; }
        public string SupplierRNC { get; set; }

        // Relationships
        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
    }
}
