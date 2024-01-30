using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Entities
{
    internal class Product
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
        
        public required string Description { get; set; }
        
        public decimal Price { get; set; }
    }
}
