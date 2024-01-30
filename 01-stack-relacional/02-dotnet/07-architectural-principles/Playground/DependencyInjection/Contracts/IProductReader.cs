using DependencyInversionPrinciple.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Contracts;

public interface IProductReader
{
    Task<IEnumerable<Product>> GetAllProducts();

    Task<Product?> GetProduct(Guid productId);
}
