using DependencyInversionPrinciple.Entities;

namespace DependencyInversionPrinciple.Contracts;

public interface IBuyerService
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product?> GetProduct(Guid productId);
}