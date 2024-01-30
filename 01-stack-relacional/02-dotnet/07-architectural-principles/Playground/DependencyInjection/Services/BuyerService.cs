using DependencyInversionPrinciple.Contracts;
using DependencyInversionPrinciple.Entities;

namespace DependencyInversionPrinciple.Services;

public class BuyerService : IBuyerService
{

    private readonly IProductReader _productRepository;

    public BuyerService(IProductReader productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IEnumerable<Product>> GetAllProducts() => _productRepository.GetAllProducts();

    public Task<Product?> GetProduct(Guid productId) => _productRepository.GetProduct(productId);

}
