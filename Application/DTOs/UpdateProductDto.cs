using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; } = string.Empty;

        public int Stock { get; set; }
        public int CategoryId { get; set; } // ID de la categoría
        public int ProductTypeId { get; set; }
    }
}
