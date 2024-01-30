using DependencyInversionPrinciple.Contracts;
using DependencyInversionPrinciple.Entities;

namespace DependencyInversionPrinciple.Services
{
    internal class BuyerService
    {

        private readonly IProductRepository _productRepository;

        public BuyerService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Product>> GetAllProducts() => _productRepository.GetAllProducts();

        public Task<Product?> GetProduct(Guid productId) => _productRepository.GetProduct(productId);

    }
}
