﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Types
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
