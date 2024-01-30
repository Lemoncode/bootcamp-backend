using DependencyInversionPrinciple.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Contracts
{
    internal interface IProductRepository
    {
        Task AddProduct(Product product);
        Task EditProduct(Product product);
        Task DeleteProduct(Guid productId);

        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product?> GetProduct(Guid productId);
    }
}
