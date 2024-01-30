using DependencyInversionPrinciple.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Contracts
{
    public interface IProductWriter
    {
        Task AddProduct(Product product);
        Task EditProduct(Product product);
        Task DeleteProduct(Guid productId);
    }
}
