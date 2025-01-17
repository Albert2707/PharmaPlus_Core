using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Context
{
    public class ProductFilterResult
    {
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; } = string.Empty;

        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
    }
}
