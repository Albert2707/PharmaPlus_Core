using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int Stock { get; set; }
        public string ImgUrl { get; set; }
        public string publicImgId { get; set; }

        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool InsuranceCoverage { get; set; } = false;

        // Relationship with Category
        public Category Category { get; set; }

        // Relationship with Type
        public Types Type { get; set; }
    }
}
