using DependencyInversionPrinciple.Entities;

namespace DependencyInversionPrinciple.Contracts
{
    public interface ISellerService
    {
        Task AddProduct(Product product);
        Task DeleteProduct(Guid productId);
        Task EditProduct(Product product);
    }
}